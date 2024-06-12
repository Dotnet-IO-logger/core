using Diol.Share.Features;
using Microsoft.Diagnostics.Tracing;
using System;

namespace Diol.Core.TraceEventProcessors
{
    /// <summary>
    /// Represents a processor for handling trace events.
    /// </summary>
    public interface IProcessor : IObserver<TraceEvent>
    {
        /// <summary>
        /// Checks if the specified event should be processed by the processor.
        /// </summary>
        /// <param name="loggerName">The name of the logger.</param>
        /// <param name="eventName">The name of the event.</param>
        /// <returns><c>true</c> if the event should be processed; otherwise, <c>false</c>.</returns>
        bool CheckEvent(string loggerName, string eventName);

        /// <summary>
        /// Gets the log data transfer object (DTO) for the specified event.
        /// </summary>
        /// <param name="eventId">The ID of the event.</param>
        /// <param name="value">The trace event.</param>
        /// <returns>The log data transfer object (DTO) for the event.</returns>
        BaseDto GetLogDto(int eventId, TraceEvent value);
    }
}
