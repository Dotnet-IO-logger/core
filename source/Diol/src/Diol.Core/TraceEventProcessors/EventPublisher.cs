using Diol.Share.Features;
using System;
using System.Collections.Generic;

namespace Diol.Core.TraceEventProcessors
{
    /// <summary>
    /// Represents an event publisher that notifies observers when a new event is added.
    /// </summary>
    public class EventPublisher : IObservable<BaseDto>
    {
        private readonly List<IObserver<BaseDto>> observers = new List<IObserver<BaseDto>>();

        /// <summary>
        /// Subscribes an observer to receive notifications when a new event is added.
        /// </summary>
        /// <param name="observer">The observer to subscribe.</param>
        /// <returns>An IDisposable object that can be used to unsubscribe the observer.</returns>
        public IDisposable Subscribe(IObserver<BaseDto> observer)
        {
            if (!this.observers.Contains(observer))
            {
                this.observers.Add(observer);
            }

            return new EventPublisherUnSubscriber(observers);
        }

        /// <summary>
        /// Adds a new event and notifies all observers.
        /// </summary>
        /// <param name="baseEvent">The event to add.</param>
        public void AddEvent(BaseDto baseEvent)
        {
            // notify all observers
            foreach (var observer in this.observers)
            {
                observer.OnNext(baseEvent);
            }
        }
    }
}
