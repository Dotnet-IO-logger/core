using Prism.Events;

namespace Diol.Wpf.Core.Features.Aspnetcores
{
    /// <summary>
    /// Represents an event that is raised when an ASP.NET request starts.
    /// </summary>
    public class AspnetRequestStartedEvent : PubSubEvent<string>
    {
    }

    /// <summary>
    /// Represents an event that is raised when an ASP.NET request ends.
    /// </summary>
    public class AspnetRequestEndedEvent : PubSubEvent<string>
    {
    }

    /// <summary>
    /// Represents an event that is raised when an item is selected in ASP.NET.
    /// </summary>
    public class AspnetItemSelectedEvent : PubSubEvent<string>
    {
    }
}
