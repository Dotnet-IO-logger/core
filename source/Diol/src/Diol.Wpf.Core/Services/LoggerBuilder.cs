using Diol.Core.Consumers;
using Diol.Core.DiagnosticClients;
using Diol.Core.TraceEventProcessors;
using Diol.Share.Features.Aspnetcores;
using Diol.Share.Features.Httpclients;
using Diol.Share.Features;
using System.Collections.Generic;

namespace Diol.Wpf.Core.Services
{
    public class LoggerBuilder
    {
        private LogsConsumer logsConsumer;

        public LoggerBuilder(LogsConsumer logsConsumer)
        {
            this.logsConsumer = logsConsumer;
        }

        public EventPipeEventSourceBuilder Build() 
        {
            return new EventPipeEventSourceBuilder()
                .SetProviders(EvenPipeHelper.Providers)
                .SetFeatures(new List<BaseFeatureFlag>()
                {
                        new AspnetcoreFeatureFlag(),
                        new HttpclientFeatureFlag()
                })
                .SetEventObserver(new EventPublisher())
                .SetConsumers(new List<IConsumer>()
                {
                        this.logsConsumer
                });
        }
    }
}
