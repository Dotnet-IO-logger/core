using Diol.Share.Features;
using System;
using System.Collections.Generic;

namespace Diol.Core.TraceEventProcessors
{
    public class EventPublisherUnsubscriber : IDisposable
    {
        private List<IObserver<BaseDto>> observers;

        public EventPublisherUnsubscriber(List<IObserver<BaseDto>> observers)
        {
            this.observers = observers;
        }

        public void Dispose()
        {
            if (observers != null) 
            {
                this.observers.Clear();
            }
        }
    }
}
