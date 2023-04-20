using Diol.Share.Features;

namespace Diol.Core.TraceEventProcessors
{
    public class EventPublisher : IObservable<BaseDto>
    {
        private readonly List<IObserver<BaseDto>> observers = new List<IObserver<BaseDto>>();

        public IDisposable Subscribe(IObserver<BaseDto> observer)
        {
            if (!this.observers.Contains(observer))
            {
                this.observers.Add(observer);
            }

            return new EventPublisherUnsubscriber(observers);
        }

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
