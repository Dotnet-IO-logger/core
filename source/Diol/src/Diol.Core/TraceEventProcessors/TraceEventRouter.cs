using Microsoft.Diagnostics.Tracing;
using System;
using System.Collections.Generic;

namespace Diol.Core.TraceEventProcessors
{
    public class TraceEventRouter : IObservable<TraceEvent>
    {
        private IProcessor httpClientProcessor;
        private IProcessor aspnetcoreProcessor;
        private IProcessor entityFrameworkProcessor;

        public TraceEventRouter(
            HttpclientProcessor httpClientProcessor,
            AspnetcoreProcessor aspnetcoreProcessor,
            EntityFrameworkProcessor entityFrameworkProcessor)
        {
            this.httpClientProcessor = httpClientProcessor;
            this.aspnetcoreProcessor = aspnetcoreProcessor;
            this.entityFrameworkProcessor = entityFrameworkProcessor;
        }

        public IDisposable Subscribe(IObserver<TraceEvent> observer)
        {
            return new TraceEventRouterUnsubscriber(new List<IObserver<TraceEvent>>() 
            {
                this.httpClientProcessor,
                this.aspnetcoreProcessor
            });
        }

        public void TraceEvent(TraceEvent traceEvent)
        {
            // route traceEvent to the correct processor
            var loggerName = traceEvent.PayloadByName("LoggerName")?.ToString();

            if (loggerName == null)
                return;

            if (this.httpClientProcessor != null
                && this.httpClientProcessor.CheckLoggerName(loggerName)
                && this.httpClientProcessor.CheckEventName(traceEvent.EventName)) 
            {
                this.httpClientProcessor.OnNext(traceEvent);
            }
            else if(this.aspnetcoreProcessor != null
                && this.aspnetcoreProcessor.CheckLoggerName(loggerName)
                && this.aspnetcoreProcessor.CheckEventName(traceEvent.EventName))
            {
                this.aspnetcoreProcessor.OnNext(traceEvent);
            }
            else if(this.entityFrameworkProcessor != null
                && this.entityFrameworkProcessor.CheckLoggerName(loggerName)
                && this.entityFrameworkProcessor.CheckEventName(traceEvent.EventName))
            {
                this.entityFrameworkProcessor.OnNext(traceEvent);
            }
        }   
    }
}
