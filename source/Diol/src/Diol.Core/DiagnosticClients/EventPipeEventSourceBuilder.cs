using Diol.Core.TraceEventProcessors;
using Diol.Share.Consumers;
using Microsoft.Diagnostics.NETCore.Client;
using System.Collections.Generic;
using System.Linq;

namespace Diol.Core.DiagnosticClients
{
    /// <summary>
    /// Builder class for creating an EventPipeEventSourceWrapper.
    /// </summary>
    public class EventPipeEventSourceBuilder
    {
        private IProcessorFactory processorFactory;

        /// <summary>
        /// Gets or sets the process ID for the EventPipeEventSource.
        /// </summary>
        public int ProcessId { get; private set; }

        /// <summary>
        /// Sets the process ID for the EventPipeEventSource.
        /// </summary>
        /// <param name="processId">The process ID.</param>
        /// <returns>The EventPipeEventSourceBuilder instance.</returns>
        public EventPipeEventSourceBuilder SetProcessId(int processId)
        {
            this.ProcessId = processId;
            return this;
        }

        /// <summary>
        /// Gets the list of EventPipeProviders.
        /// </summary>
        public List<EventPipeProvider> Providers => EventPipeHelper.Providers.ToList();

        /// <summary>
        /// Gets or sets the list of consumers.
        /// </summary>
        public IEnumerable<IConsumer> Consumers { get; private set; }

        /// <summary>
        /// Gets or sets the event observer.
        /// </summary>
        public EventPublisher EventObserver { get; private set; }

        /// <summary>
        /// Initializes a new instance of the EventPipeEventSourceBuilder class.
        /// </summary>
        /// <param name="consumers">The list of consumers.</param>
        /// <param name="processorFactory">The processor factory.</param>
        /// <param name="eventObserver">The event observer.</param>
        public EventPipeEventSourceBuilder(
            IEnumerable<IConsumer> consumers,
            IProcessorFactory processorFactory,
            EventPublisher eventObserver)
        {
            this.Consumers = consumers;
            this.processorFactory = processorFactory;
            this.EventObserver = eventObserver;
        }

        /// <summary>
        /// Builds the EventPipeEventSourceWrapper.
        /// </summary>
        /// <returns>The EventPipeEventSourceWrapper instance.</returns>
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
