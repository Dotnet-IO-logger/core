using Diol.Share.Features;
using System;
using System.Collections.Generic;

namespace Diol.Core.TraceEventProcessors
{
    /// <summary>
    /// Represents a class that unsubscribes observers from an event publisher.
    /// </summary>
    public class EventPublisherUnSubscriber : IDisposable
    {
        private List<IObserver<BaseDto>> observers;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventPublisherUnSubscriber"/> class.
        /// </summary>
        /// <param name="observers">The list of observers to unsubscribe.</param>
        public EventPublisherUnSubscriber(List<IObserver<BaseDto>> observers)
        {
            this.observers = observers;
        }

        /// <summary>
        /// Disposes the <see cref="EventPublisherUnSubscriber"/> instance and clears the list of observers.
        /// </summary>
        public void Dispose()
        {
            if (observers != null)
            {
                this.observers.Clear();
            }
        }
    }
}
