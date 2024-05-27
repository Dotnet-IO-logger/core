using Diol.Aspnet.Hubs;
using Diol.Core.DiagnosticClients;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System.Threading.Channels;

namespace Diol.Aspnet.BackgroundWorkers
{
    /// <summary>
    /// Represents a queue for background tasks.
    /// </summary>
    public class BackgroundTaskQueue
    {
        private readonly Channel<Func<EventPipeEventSourceBuilder, CancellationToken, ValueTask>> _queue;
        private readonly IHubContext<LogsHub> hubContext;
        private readonly ILogger<BackgroundTaskQueue> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="BackgroundTaskQueue"/> class.
        /// </summary>
        /// <param name="hubContext">The hub context.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="capacity">The capacity of the queue. Default is 5.</param>
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

        /// <summary>
        /// Queues a background work item for execution.
        /// </summary>
        /// <param name="workItem">The work item to be executed.</param>
        /// <returns>A <see cref="ValueTask"/> representing the asynchronous operation.</returns>
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

        /// <summary>
        /// Queues logs processing for a specific process ID.
        /// </summary>
        /// <param name="processId">The process ID.</param>
        /// <returns>A <see cref="ValueTask"/> representing the asynchronous operation.</returns>
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
                    Task processing = Task.Run(() =>
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

        /// <summary>
        /// Dequeues a work item from the queue.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A <see cref="Func{T1, T2, TResult}"/> representing the work item.</returns>
        public async ValueTask<Func<EventPipeEventSourceBuilder, CancellationToken, ValueTask>> DequeueAsync(
            CancellationToken cancellationToken)
        {
            var workItem = await _queue.Reader.ReadAsync(cancellationToken);

            return workItem;
        }
    }
}
