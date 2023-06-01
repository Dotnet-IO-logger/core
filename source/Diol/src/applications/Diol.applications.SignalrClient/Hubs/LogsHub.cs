using Diol.applications.SignalrClient.BackgroundWorkers;
using Diol.Core.DotnetProcesses;
using Microsoft.AspNetCore.SignalR;

namespace Diol.applications.SignalrClient.Hubs
{
    public class LogsHub : Hub
    {
        private readonly BackgroundTaskQueue taskQueue;
        private readonly DotnetProcessesService dotnetProcessesService;

        public LogsHub(
            BackgroundTaskQueue taskQueue, 
            DotnetProcessesService dotnetProcessesService)
        {
            this.taskQueue = taskQueue;
            this.dotnetProcessesService = dotnetProcessesService;
        }

        public async Task GetProcesses(string message)
        {
            var processes = this.dotnetProcessesService.GetCollection();

            await this.Clients.Caller
                .SendAsync("GetProcesses", processes);
        }

        public async Task Subscribe(int processId)
        {
            await this.Groups.AddToGroupAsync(
                this.Context.ConnectionId, 
                processId.ToString());

            await this.taskQueue.QueueLogsProcessing(processId);
        }
    }
}
