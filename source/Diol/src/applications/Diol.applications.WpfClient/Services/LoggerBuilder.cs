using Diol.Core.Consumers;
using Diol.Core.DiagnosticClients;
using Diol.Core.TraceEventProcessors;
using Diol.Share.Features.Aspnetcores;
using Diol.Share.Features.Httpclients;
using Diol.Share.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diol.applications.WpfClient.Services
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
