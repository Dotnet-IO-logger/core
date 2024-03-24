using Diol.Core.Consumers;
using Diol.Core.TraceEventProcessors;
using Diol.Share.Features;
using Diol.Share.Features.Aspnetcores;
using Diol.Share.Features.Httpclients;
using Microsoft.Diagnostics.NETCore.Client;
using System.Collections.Generic;
using System.Linq;

namespace Diol.Core.DiagnosticClients
{
    public class EventPipeEventSourceBuilder
    {
        public static EventPipeEventSourceBuilder CreateDefault() =>
            new EventPipeEventSourceBuilder()
                .SetProviders(EvenPipeHelper.Providers)
                .SetEventObserver(new EventPublisher());

        public int ProcessId { get; private set; }
        public EventPipeEventSourceBuilder SetProcessId(int processId)
        {
            this.ProcessId = processId;
            return this;
        }

        public List<EventPipeProvider> Providers { get; private set; } = new List<EventPipeProvider>();
        public EventPipeEventSourceBuilder SetProviders(IReadOnlyCollection<EventPipeProvider> providers)
        {
            this.Providers.Clear();
            this.Providers = providers.ToList();
            return this;
        }

        public List<IConsumer> Consumers { get; private set; } = new List<IConsumer>();
        public EventPipeEventSourceBuilder SetConsumers(List<IConsumer> consumers)
        {
            this.Consumers.Clear();
            this.Consumers = consumers;
            return this;
        }

        public EventPublisher EventObserver { get; private set; }
        public EventPipeEventSourceBuilder SetEventObserver(EventPublisher eventObserver)
        {
            this.EventObserver = eventObserver;
            return this;
        }

        public EventPipeEventSourceWrapper Build() 
        {
            foreach (var consumer in this.Consumers)
                this.EventObserver.Subscribe(consumer);

            return new EventPipeEventSourceWrapper(
                this, 
                new TraceEventRouter(
                    new HttpclientProcessor(this.EventObserver),
                    new AspnetcoreProcessor(this.EventObserver),
                    new EntityFrameworkProcessor(this.EventObserver)));
        }
    }
}
