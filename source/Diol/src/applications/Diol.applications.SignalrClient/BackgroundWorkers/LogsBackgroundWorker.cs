using Diol.applications.SignalrClient.Consumers;
using Diol.applications.SignalrClient.Hubs;
using Diol.Core.DiagnosticClients;
using Diol.Core.TraceEventProcessors;
using Diol.Share.Features.Aspnetcores;
using Diol.Share.Features.Httpclients;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace Diol.applications.SignalrClient.BackgroundWorkers
{
    public class LogsBackgroundWorker : BackgroundService
    {
        private readonly BackgroundTaskQueue taskQueue;
        private readonly SignalRConsumer signalRConsumer;

        private EventPipeEventSourceBuilder builder;

        public LogsBackgroundWorker(
            BackgroundTaskQueue taskQueue,
            SignalRConsumer signalRConsumer)
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
                .SetEventObserver(new EventPublisher())
                .SetConsumers(new List<Core.Consumers.IConsumer>()
                {
                    this.signalRConsumer
                });
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested) 
            {
                var workItem = await this.taskQueue
                    .DequeueAsync(stoppingToken)
                    .ConfigureAwait(false);

                _ = workItem(this.builder, stoppingToken)
                    .ConfigureAwait(false);
            }
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return base.StopAsync(cancellationToken);
        }
    }
}
