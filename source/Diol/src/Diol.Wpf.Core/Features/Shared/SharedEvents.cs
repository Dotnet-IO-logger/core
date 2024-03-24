using Diol.Share.Utils;
using Prism.Events;
using System.Collections.Generic;

namespace Diol.Wpf.Core.Features.Shared
{
    public class ClearDataEvent : PubSubEvent<string>
    {
    }

    public class DebugModeRunnedEvent : PubSubEvent<bool> 
    {
    }

    public class DiagnosticEvent : PubSubEvent<DiagnosticModel>
    {
    }

    public class DiagnosticModel 
    {
        public string CategoryName { get; set; }

        public string EventName { get; set; }

        public string ActivityId { get; set; }
    }

    public class ProcessesReceivedEvent : PubSubEvent<ICollection<DotnetProcessInfo>>
    {
    }

    public enum SignalRConnectionEnum 
    {
        Connecting,
        Connected,
        Reconnecting,
        Reconnected,
        Closed,
        Error
    }

    public class SignalRConnectionEvent : PubSubEvent<SignalRConnectionEnum>
    {
    }

    public class ProcessStarted : PubSubEvent<int>
    {
    }

    public class ProcessFinished : PubSubEvent<int>
    {
    }

    public class BackendStarted : PubSubEvent<bool>
    {
    }
}
