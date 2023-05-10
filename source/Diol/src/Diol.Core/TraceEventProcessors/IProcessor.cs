using Microsoft.Diagnostics.Tracing;
using System;

namespace Diol.Core.TraceEventProcessors
{
    public interface IProcessor : IObserver<TraceEvent>
    {
        bool CheckLoggerName(string name);

        bool CheckEventName(string eventName);
    }
}
