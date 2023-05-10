using Microsoft.Diagnostics.Tracing;
using System;
using System.Collections.Generic;

namespace Diol.Core.TraceEventProcessors
{
    public class TraceEventRouterUnsubscriber : IDisposable
    {
        private List<IObserver<TraceEvent>> observers;

        public TraceEventRouterUnsubscriber(List<IObserver<TraceEvent>> observers)
        {
            this.observers = observers;
        }

        public void Dispose()
        {
            if(this.observers != null) 
            {
                this.observers.Clear();
            }
        }
    }
}
