using Diol.applications.SignalrClient.Consumers;
using Diol.applications.SignalrClient.Hubs;
using Diol.Core.DiagnosticClients;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Channels;

namespace Diol.applications.SignalrClient.BackgroundWorkers
{
    public class BackgroundTaskQueue
    {
        private readonly Channel<Func<EventPipeEventSourceBuilder, CancellationToken, ValueTask>> _queue;

        private readonly IHubContext<LogsHub> hubContext;

        public BackgroundTaskQueue(IHubContext<LogsHub> hubContext, int capacity = 1)
        {
            this.hubContext = hubContext;

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
                            await Task.Delay(TimeSpan.FromSeconds(5));
                        }

                        executor.Stop();

                        return Task.CompletedTask;
                    });

                    // do processing
                    Task processing = Task.Run(async () =>
                    {
                        executor.Start();

                        executor.Dispose();
                    });

                    Task.WaitAny(finish, processing);

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
