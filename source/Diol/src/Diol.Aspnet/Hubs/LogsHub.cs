using Diol.Aspnet.BackgroundWorkers;
using Diol.Share.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Diol.Aspnet.Hubs
{
    public class LogsHub : Hub
    {
        private readonly BackgroundTaskQueue taskQueue;
        private readonly DotnetProcessesService dotnetProcessesService;
        private readonly ILogger<LogsHub> logger;

        public LogsHub(
            BackgroundTaskQueue taskQueue,
            DotnetProcessesService dotnetProcessesService,
            ILogger<LogsHub> logger)
        {
            this.taskQueue = taskQueue;
            this.dotnetProcessesService = dotnetProcessesService;
            this.logger = logger;
        }

        public async Task GetProcesses(string message)
        {
            var processes = this.dotnetProcessesService.GetCollection();

            await this.Clients.Caller
                .SendAsync("ProcessesReceived", processes);
        }

        public async Task Subscribe(int processId)
        {
            await this.Groups.AddToGroupAsync(
                this.Context.ConnectionId,
                processId.ToString());

            await this.Clients.Caller
                .SendAsync("ProcessingStarted", processId);

            await this.taskQueue.QueueLogsProcessing(processId);
        }

        public override async Task OnConnectedAsync()
        {
            this.logger.LogInformation(
                "Client connected: {connectionId}",
                this.Context.ConnectionId);

            await this.Clients.Caller
                .SendAsync("DiolLogsHubConnected");

            await base.OnConnectedAsync();
        }
    }
}
