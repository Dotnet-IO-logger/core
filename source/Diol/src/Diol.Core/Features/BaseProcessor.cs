using Diol.Core.TraceEventProcessors;
using Diol.Share.Features;
using Microsoft.Diagnostics.Tracing;
using System;
using System.Diagnostics;

namespace Diol.Core.Features
{
    public abstract class BaseProcessor : IProcessor
    {
        private readonly EventPublisher eventObserver;

        public BaseProcessor(EventPublisher eventObserver)
        {
            this.eventObserver = eventObserver;
        }

        public void OnCompleted()
        {
            Debug.WriteLine($"{nameof(BaseProcessor)} | {nameof(OnCompleted)}");
        }

        public void OnError(Exception error)
        {
            Debug.WriteLine($"{nameof(BaseProcessor)} | {nameof(OnError)} | {error.Message}");
        }

        public void OnNext(TraceEvent value)
        {
            // if you run the app via VS-> for responce's activity ids will be incorrect.
            var eventId = Convert.ToInt32(value.PayloadByName("EventId"));
            var eventName = value.PayloadByName("EventName")?.ToString();

            var dto = GetLogDto(eventId, value);

            if (dto == null)
                return;

            dto.ProcessName = value.ProcessName;
            dto.ProcessId = value.ProcessID;

            eventObserver.AddEvent(dto);
        }

        public abstract BaseDto GetLogDto(int eventId, TraceEvent value);

        public abstract bool CheckEvent(string loggerName, string eventName);
    }
}
