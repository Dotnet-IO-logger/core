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
        private readonly SignalRConsumer signalRConsumer;
        private EventPipeEventSourceBuilder builder;
        private readonly ILogger<LogsBackgroundWorker> logger;

        public LogsBackgroundWorker(
            BackgroundTaskQueue taskQueue,
            SignalRConsumer signalRConsumer,
            EventPublisher eventPublisher,
            ILogger<LogsBackgroundWorker> logger)
        {
            this.taskQueue = taskQueue;
            this.signalRConsumer = signalRConsumer;
            this.builder = new EventPipeEventSourceBuilder()
                .SetProviders(EvenPipeHelper.Providers)
                .SetFeatures(new List<Share.Features.BaseFeatureFlag>()
                {
                    new AspnetcoreFeatureFlag(),
                    new HttpclientFeatureFlag()
                })
                .SetEventObserver(eventPublisher)
                .SetConsumers(new List<Core.Consumers.IConsumer>()
                {
                    this.signalRConsumer
                });
            this.logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            this.logger.LogInformation("LogsBackgroundWorker Execution started");
            while (!stoppingToken.IsCancellationRequested)
            {
                var workItem = await this.taskQueue
                    .DequeueAsync(stoppingToken)
                    .ConfigureAwait(false);

                this.logger.LogInformation("LogsBackgroundWorker work item queued");

                await workItem(this.builder, stoppingToken)
                    .ConfigureAwait(false);

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
