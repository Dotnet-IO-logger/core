using Microsoft.Diagnostics.Tracing;
using System;
using System.Collections.Generic;

namespace Diol.Core.TraceEventProcessors
{
    /// <summary>
    /// Represents a router for trace events.
    /// </summary>
    public class TraceEventRouter : IObservable<TraceEvent>
    {
        private readonly IProcessorFactory processorFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="TraceEventRouter"/> class.
        /// </summary>
        /// <param name="processorFactory">The processor factory.</param>
        public TraceEventRouter(
            IProcessorFactory processorFactory)
        {
            this.processorFactory = processorFactory;
        }

        /// <summary>
        /// Subscribes an observer to receive trace events.
        /// </summary>
        /// <param name="observer">The observer to subscribe.</param>
        /// <returns>An <see cref="IDisposable"/> representing the subscription.</returns>
        public IDisposable Subscribe(IObserver<TraceEvent> observer) =>
            new TraceEventRouterUnSubscriber(
                new List<IObserver<TraceEvent>>(
                    this.processorFactory.GetProcessors()));

        /// <summary>
        /// Routes a trace event to the correct processor.
        /// </summary>
        /// <param name="traceEvent">The trace event to route.</param>
        public void TraceEvent(TraceEvent traceEvent)
        {
            // route traceEvent to the correct processor
            var loggerName = traceEvent.PayloadByName("LoggerName")?.ToString();

            if (loggerName == null)
                return;

            var processor = this.processorFactory.GetProcessor(
                loggerName,
                traceEvent.EventName);

            if (processor == null)
                return;

            processor.OnNext(traceEvent);
        }
    }
}
