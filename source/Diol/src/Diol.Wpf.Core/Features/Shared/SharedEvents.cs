using Diol.Share.Utils;
using Prism.Events;
using System.Collections.Generic;

namespace Diol.Wpf.Core.Features.Shared
{
    /// <summary>
    /// Event that is raised to clear data.
    /// </summary>
    public class ClearDataEvent : PubSubEvent<string>
    {
    }

    /// <summary>
    /// Event that is raised when debug mode is run.
    /// </summary>
    public class DebugModeRunnedEvent : PubSubEvent<bool>
    {
    }

    /// <summary>
    /// Event that is raised for diagnostic purposes.
    /// </summary>
    public class DiagnosticEvent : PubSubEvent<DiagnosticModel>
    {
    }

    /// <summary>
    /// Model for diagnostic information.
    /// </summary>
    public class DiagnosticModel
    {
        /// <summary>
        /// Gets or sets the category name.
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        /// Gets or sets the event name.
        /// </summary>
        public string EventName { get; set; }

        /// <summary>
        /// Gets or sets the activity ID.
        /// </summary>
        public string ActivityId { get; set; }
    }

    /// <summary>
    /// Event that is raised when processes are received.
    /// </summary>
    public class ProcessesReceivedEvent : PubSubEvent<ICollection<DotnetProcessInfo>>
    {
    }

    /// <summary>
    /// Enum representing the SignalR connection status.
    /// </summary>
    public enum SignalRConnectionEnum
    {
        Connecting,
        Connected,
        Reconnecting,
        Reconnected,
        Closed,
        Error
    }

    /// <summary>
    /// Event that is raised for SignalR connection status changes.
    /// </summary>
    public class SignalRConnectionEvent : PubSubEvent<SignalRConnectionEnum>
    {
    }

    /// <summary>
    /// Event that is raised when a process is started.
    /// </summary>
    public class ProcessStarted : PubSubEvent<int>
    {
    }

    /// <summary>
    /// Event that is raised when a process is finished.
    /// </summary>
    public class ProcessFinished : PubSubEvent<int>
    {
    }

    /// <summary>
    /// Event that is raised when the backend is started.
    /// </summary>
    public class BackendStarted : PubSubEvent<bool>
    {
    }
}
