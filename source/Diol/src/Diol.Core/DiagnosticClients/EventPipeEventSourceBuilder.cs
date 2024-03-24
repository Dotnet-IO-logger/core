using Diol.Core.TraceEventProcessors;
using Diol.Share.Consumers;
using Microsoft.Diagnostics.NETCore.Client;
using System.Collections.Generic;
using System.Linq;

namespace Diol.Core.DiagnosticClients
{
    public class EventPipeEventSourceBuilder
    {
        private IProcessorFactory processorFactory;

        public int ProcessId { get; private set; }
        public EventPipeEventSourceBuilder SetProcessId(int processId)
        {
            this.ProcessId = processId;
            return this;
        }

        public List<EventPipeProvider> Providers => EvenPipeHelper.Providers.ToList();

        public IEnumerable<IConsumer> Consumers { get; private set; }

        public EventPublisher EventObserver { get; private set; }

        public EventPipeEventSourceBuilder(
            IEnumerable<IConsumer> consumers,
            IProcessorFactory processorFactory,
            EventPublisher eventObserver)
        {
            this.Consumers = consumers;
            this.processorFactory = processorFactory;
            this.EventObserver = eventObserver;
        }

        public EventPipeEventSourceWrapper Build() 
        {
            foreach (var consumer in this.Consumers)
                this.EventObserver.Subscribe(consumer);

            return new EventPipeEventSourceWrapper(
                this, 
                new TraceEventRouter(this.processorFactory));
        }
    }
}
