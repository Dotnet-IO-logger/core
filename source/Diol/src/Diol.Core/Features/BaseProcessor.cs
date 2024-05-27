using Diol.Core.TraceEventProcessors;
using Diol.Share.Features;
using Microsoft.Diagnostics.Tracing;
using System;
using System.Diagnostics;

namespace Diol.Core.Features
{
    /// <summary>
    /// Represents a base processor for handling trace events.
    /// </summary>
    public abstract class BaseProcessor : IProcessor
    {
        private readonly EventPublisher eventObserver;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseProcessor"/> class.
        /// </summary>
        /// <param name="eventObserver">The event observer.</param>
        public BaseProcessor(EventPublisher eventObserver)
        {
            this.eventObserver = eventObserver;
        }

        /// <summary>
        /// Handles the completion of the trace event processing.
        /// </summary>
        public void OnCompleted()
        {
            Debug.WriteLine($"{nameof(BaseProcessor)} | {nameof(OnCompleted)}");
        }

        /// <summary>
        /// Handles the error occurred during the trace event processing.
        /// </summary>
        /// <param name="error">The error that occurred.</param>
        public void OnError(Exception error)
        {
            Debug.WriteLine($"{nameof(BaseProcessor)} | {nameof(OnError)} | {error.Message}");
        }

        /// <summary>
        /// Handles the next trace event.
        /// </summary>
        /// <param name="value">The trace event.</param>
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

        /// <summary>
        /// Gets the log data transfer object for the specified event.
        /// </summary>
        /// <param name="eventId">The event ID.</param>
        /// <param name="value">The trace event.</param>
        /// <returns>The log data transfer object.</returns>
        public abstract BaseDto GetLogDto(int eventId, TraceEvent value);

        /// <summary>
        /// Checks if the specified event should be processed.
        /// </summary>
        /// <param name="loggerName">The logger name.</param>
        /// <param name="eventName">The event name.</param>
        /// <returns><c>true</c> if the event should be processed; otherwise, <c>false</c>.</returns>
        public abstract bool CheckEvent(string loggerName, string eventName);
    }
}
