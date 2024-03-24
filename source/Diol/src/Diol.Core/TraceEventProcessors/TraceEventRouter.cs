using Microsoft.Diagnostics.Tracing;
using System;
using System.Collections.Generic;

namespace Diol.Core.TraceEventProcessors
{
    public class TraceEventRouter : IObservable<TraceEvent>
    {
        private readonly IProcessorFactory processorFactory;

        public TraceEventRouter(
            IProcessorFactory processorFactory)
        {
            this.processorFactory = processorFactory;
        }

        public IDisposable Subscribe(IObserver<TraceEvent> observer) =>
            new TraceEventRouterUnsubscriber(
                new List<IObserver<TraceEvent>>(
                    this.processorFactory.GetProcessors()));

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
