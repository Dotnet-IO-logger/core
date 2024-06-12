using Diol.Aspnet.BackgroundWorkers;
using Diol.Share.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Diol.Aspnet.Hubs
{
    /// <summary>
    /// Represents a SignalR hub for managing logs.
    /// </summary>
    public class LogsHub : Hub
    {
        private readonly BackgroundTaskQueue taskQueue;
        private readonly DotnetProcessesService dotnetProcessesService;
        private readonly ILogger<LogsHub> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogsHub"/> class.
        /// </summary>
        /// <param name="taskQueue">The background task queue.</param>
        /// <param name="dotnetProcessesService">The DotnetProcessesService instance.</param>
        /// <param name="logger">The logger instance.</param>
        public LogsHub(
            BackgroundTaskQueue taskQueue,
            DotnetProcessesService dotnetProcessesService,
            ILogger<LogsHub> logger)
        {
            this.taskQueue = taskQueue;
            this.dotnetProcessesService = dotnetProcessesService;
            this.logger = logger;
        }

        /// <summary>
        /// Gets the processes.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task GetProcesses(string message)
        {
            var processes = this.dotnetProcessesService.GetCollection();

            await this.Clients.Caller
                .SendAsync("ProcessesReceived", processes);
        }

        /// <summary>
        /// Subscribes to a process.
        /// </summary>
        /// <param name="processId">The process ID.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task Subscribe(int processId)
        {
            await this.Groups.AddToGroupAsync(
                this.Context.ConnectionId,
                processId.ToString());

            await this.Clients.Caller
                .SendAsync("ProcessingStarted", processId);

            await this.taskQueue.QueueLogsProcessing(processId);
        }

        /// <summary>
        /// Called when a client is connected.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
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
