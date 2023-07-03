using Diol.Aspnet.Hubs;
using Diol.Core.DiagnosticClients;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System.Threading.Channels;

namespace Diol.Aspnet.BackgroundWorkers
{
    public class BackgroundTaskQueue
    {
        private readonly Channel<Func<EventPipeEventSourceBuilder, CancellationToken, ValueTask>> _queue;
        private readonly IHubContext<LogsHub> hubContext;
        private readonly ILogger<BackgroundTaskQueue> logger;

        public BackgroundTaskQueue(
            IHubContext<LogsHub> hubContext,
            ILogger<BackgroundTaskQueue> logger,
            int capacity = 5)
        {
            this.hubContext = hubContext;
            this.logger = logger;

            // Capacity should be set based on the expected application load and
            // number of concurrent threads accessing the queue.            
            // BoundedChannelFullMode.Wait will cause calls to WriteAsync() to return a task,
            // which completes only when space became available. This leads to backpressure,
            // in case too many publishers/calls start accumulating.
            var options = new BoundedChannelOptions(capacity)
            {
                FullMode = BoundedChannelFullMode.Wait
            };
            _queue = Channel.CreateBounded<Func<EventPipeEventSourceBuilder, CancellationToken, ValueTask>>(options);
        }

        public async ValueTask QueueBackgroundWorkItemAsync(
            Func<EventPipeEventSourceBuilder, CancellationToken, ValueTask> workItem)
        {
            if (workItem == null)
            {
                this.logger.LogError(
                    "QueueBackgroundWorkItemAsync args is null: {argName}", 
                    nameof(workItem));

                throw new ArgumentNullException(nameof(workItem));
            }

            await _queue.Writer.WriteAsync(workItem);
        }

        public async ValueTask QueueLogsProcessing(int processId)
        {
            await this.QueueBackgroundWorkItemAsync(
                async (builder, cancelationToken) =>
                {
                    var executor = builder
                        .SetProcessId(processId)
                        .Build();

                    // finish action
                    Task finish = Task.Run(async () =>
                    {
                        while (!cancelationToken.IsCancellationRequested)
                        {
                            await Task.Delay(TimeSpan.FromSeconds(2));
                        }

                        executor.Stop();

                        this.logger.LogInformation(
                            "Processing stopped. ProcessId: {processId}", 
                            processId);

                        return Task.CompletedTask;
                    });

                    // do processing
                    Task processing = Task.Run(async () =>
                    {
                        this.logger.LogInformation(
                            "Processing started. ProcessId: {processId}", 
                            processId);

                        executor.Start();

                        this.logger.LogInformation(
                            "Processing finished. ProcessId: {processId}", 
                            processId);

                        executor.Dispose();
                    });

                    this.logger.LogInformation(
                        "General processing being. ProcessId: {processId}", 
                        processId);

                    Task.WaitAny(finish, processing);

                    this.logger.LogInformation(
                        "General processing finished. ProcessId: {processId}", 
                        processId);

                    await this.hubContext.Clients
                        .Group(processId.ToString())
                        .SendAsync("ProcessingFinished", processId);
                });
        }

        public async ValueTask<Func<EventPipeEventSourceBuilder, CancellationToken, ValueTask>> DequeueAsync(
            CancellationToken cancellationToken)
        {
            var workItem = await _queue.Reader.ReadAsync(cancellationToken);

            return workItem;
        }
    }
}
