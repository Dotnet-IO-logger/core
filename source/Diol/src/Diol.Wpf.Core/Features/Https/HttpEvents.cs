using Prism.Events;

namespace Diol.Wpf.Core.Features.Https
{
    public class HttpRequestStartedEvent : PubSubEvent<string>
    {
    }

    public class HttpRequestEndedEvent : PubSubEvent<string> 
    {
    }

    public class HttpItemSelectedEvent : PubSubEvent<string> 
    {
    }
}
