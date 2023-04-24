using Diol.applications.SignalrClient.BackgroundWorkers;
using Diol.applications.SignalrClient.Consumers;
using Diol.Core.DiagnosticClients;
using Diol.Core.TraceEventProcessors;
using Diol.Share.Features.Aspnetcores;
using Diol.Share.Features.Httpclients;
using Microsoft.AspNetCore.SignalR;

namespace Diol.applications.SignalrClient.Hubs
{
    public class LogsHub : Hub
    {
        private readonly BackgroundTaskQueue taskQueue;

        public LogsHub(BackgroundTaskQueue taskQueue)
        {
            this.taskQueue = taskQueue;
        }

        public async Task GetProcesses(string message)
        {
            // return all dotnet processes
        }

        public async Task Subscribe(int processId)
        {
            await this.taskQueue.QueueBackgroundWorkItemAsync(async (builder, cancelationToken) =>
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
                Task processing = Task.Run(() =>
                {
                    executor.Start();

                    executor.Dispose();
                });

                Task.WaitAny(finish, processing);
            });
        }

        public async Task Unsubscribe(int processId)
        {
        }
    }
}
