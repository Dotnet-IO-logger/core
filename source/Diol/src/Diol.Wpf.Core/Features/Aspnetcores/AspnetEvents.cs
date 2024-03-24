using Prism.Events;

namespace Diol.Wpf.Core.Features.Aspnetcores
{
    public class AspnetRequestStartedEvent : PubSubEvent<string>
    {
    }

    public class AspnetRequestEndedEvent : PubSubEvent<string>
    {
    }

    public class AspnetItemSelectedEvent : PubSubEvent<string>
    {
    }
}
