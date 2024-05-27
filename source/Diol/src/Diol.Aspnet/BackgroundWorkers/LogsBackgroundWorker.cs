using Diol.Core.DiagnosticClients;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Diol.Aspnet.BackgroundWorkers
{
    /// <summary>
    /// Represents a background worker for processing logs.
    /// </summary>
    public class LogsBackgroundWorker : BackgroundService
    {
        private readonly BackgroundTaskQueue taskQueue;
        private EventPipeEventSourceBuilder builder;
        private readonly ILogger<LogsBackgroundWorker> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogsBackgroundWorker"/> class.
        /// </summary>
        /// <param name="taskQueue">The background task queue.</param>
        /// <param name="builder">The event pipe event source builder.</param>
        /// <param name="logger">The logger.</param>
        public LogsBackgroundWorker(
            BackgroundTaskQueue taskQueue,
            EventPipeEventSourceBuilder builder,
            ILogger<LogsBackgroundWorker> logger)
        {
            this.taskQueue = taskQueue;
            this.builder = builder;
            this.logger = logger;
        }

        /// <summary>
        /// Executes the background worker asynchronously.
        /// </summary>
        /// <param name="stoppingToken">The cancellation token to stop the execution.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            this.logger.LogInformation("LogsBackgroundWorker Execution started");
            while (!stoppingToken.IsCancellationRequested)
            {
                var workItem = await this.taskQueue
                    .DequeueAsync(stoppingToken);

                this.logger.LogInformation("LogsBackgroundWorker work item queued");

                await workItem(this.builder, stoppingToken);

                this.logger.LogInformation("LogsBackgroundWorker work item executed");
            }
            this.logger.LogInformation("LogsBackgroundWorker Execution finished");
        }

        /// <summary>
        /// Starts the background worker asynchronously.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token to stop the execution.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            this.logger.LogInformation("LogsBackgroundWorker started");
            return base.StartAsync(cancellationToken);
        }

        /// <summary>
        /// Stops the background worker asynchronously.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token to stop the execution.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public override Task StopAsync(CancellationToken cancellationToken)
        {
            this.logger.LogInformation("LogsBackgroundWorker stopped");
            return base.StopAsync(cancellationToken);
        }
    }
}
