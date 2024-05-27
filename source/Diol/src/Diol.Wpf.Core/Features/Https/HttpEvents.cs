using Prism.Events;

namespace Diol.Wpf.Core.Features.Https
{
    /// <summary>
    /// Event raised when an HTTP request is started.
    /// </summary>
    public class HttpRequestStartedEvent : PubSubEvent<string>
    {
    }

    /// <summary>
    /// Event raised when an HTTP request is ended.
    /// </summary>
    public class HttpRequestEndedEvent : PubSubEvent<string>
    {
    }

    /// <summary>
    /// Event raised when an HTTP item is selected.
    /// </summary>
    public class HttpItemSelectedEvent : PubSubEvent<string>
    {
    }
}
