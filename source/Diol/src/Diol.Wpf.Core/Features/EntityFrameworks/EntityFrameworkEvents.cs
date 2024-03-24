using Prism.Events;

namespace Diol.Wpf.Core.Features.EntityFrameworks
{
    public class EntityFrameworkConnectionStartedEvent : PubSubEvent<string>
    {
    }

    public class EntityFrameworkConnectionEndedEvent : PubSubEvent<string>
    {
    }

    public class EntityFrameworkItemSelectedEvent : PubSubEvent<string>
    {
    }
}
