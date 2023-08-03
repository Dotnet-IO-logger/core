using Diol.Core.DiagnosticClients;
using Diol.Core.TraceEventProcessors;
using Diol.Share.Features.Aspnetcores;
using Diol.Share.Features.Httpclients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diol.Wpf.Core.Services
{
    public class DiolBuilder
    {
        private readonly EventPipeEventSourceBuilder builder;

        public DiolBuilder(
            WpfConsumer wpfConsumer,
            EventPublisher eventPublisher)
        {
            this.builder = new EventPipeEventSourceBuilder()
                .SetProviders(EvenPipeHelper.Providers)
                .SetFeatures(new List<Share.Features.BaseFeatureFlag>()
                {
                    new AspnetcoreFeatureFlag(),
                    new HttpclientFeatureFlag()
                })
                .SetEventObserver(eventPublisher)
                .SetConsumers(new List<Diol.Core.Consumers.IConsumer>()
                {
                    wpfConsumer
                });
        }

        public EventPipeEventSourceBuilder GetBuilder()
        {
            return this.builder;
        }
    }
}
