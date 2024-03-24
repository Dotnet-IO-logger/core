using Diol.Aspnet.Consumers;
using Diol.Core.DiagnosticClients;
using Diol.Core.TraceEventProcessors;
using Diol.Share.Features.Aspnetcores;
using Diol.Share.Features.Httpclients;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Diol.Aspnet.BackgroundWorkers
{
    public class LogsBackgroundWorker : BackgroundService
    {
        private readonly BackgroundTaskQueue taskQueue;
        private EventPipeEventSourceBuilder builder;
        private readonly ILogger<LogsBackgroundWorker> logger;

        public LogsBackgroundWorker(
            BackgroundTaskQueue taskQueue,
            EventPipeEventSourceBuilder builder,
            ILogger<LogsBackgroundWorker> logger)
        {
            this.taskQueue = taskQueue;
            this.builder = builder;
            this.logger = logger;
        }

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

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            this.logger.LogInformation("LogsBackgroundWorker started");
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            this.logger.LogInformation("LogsBackgroundWorker started");
            return base.StopAsync(cancellationToken);
        }
    }
}
