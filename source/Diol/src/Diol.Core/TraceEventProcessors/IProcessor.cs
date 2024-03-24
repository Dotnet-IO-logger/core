using Diol.Share.Features;
using Microsoft.Diagnostics.Tracing;
using System;

namespace Diol.Core.TraceEventProcessors
{
    public interface IProcessor : IObserver<TraceEvent>
    {
        bool CheckEvent(string loggerName, string eventName);

        BaseDto GetLogDto(int eventId, TraceEvent value);
    }
}
