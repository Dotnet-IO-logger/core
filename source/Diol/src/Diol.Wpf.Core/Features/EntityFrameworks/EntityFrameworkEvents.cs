using Prism.Events;

namespace Diol.Wpf.Core.Features.EntityFrameworks
{
    /// <summary>
    /// Represents an event that is raised when an Entity Framework connection is started.
    /// </summary>
    public class EntityFrameworkConnectionStartedEvent : PubSubEvent<string>
    {
    }

    /// <summary>
    /// Represents an event that is raised when an Entity Framework connection is started.
    /// </summary>
    public class EntityFrameworkQueryExecutingEvent : PubSubEvent<string>
    {
    }

    /// <summary>
    /// Represents an event that is raised when an Entity Framework connection is ended.
    /// </summary>
    public class EntityFrameworkConnectionEndedEvent : PubSubEvent<string>
    {
    }

    /// <summary>
    /// Represents an event that is raised when an item is selected in Entity Framework.
    /// </summary>
    public class EntityFrameworkItemSelectedEvent : PubSubEvent<string>
    {
    }
}
