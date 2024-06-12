using Microsoft.Diagnostics.NETCore.Client;
using System.Collections.Generic;
using System.Diagnostics.Tracing;

namespace Diol.Core.DiagnosticClients
{
    /// <summary>
    /// Helper class for EventPipe functionality.
    /// </summary>
    public static class EvenPipeHelper
    {
        /// <summary>
        /// Gets a collection of EventPipeProvider instances.
        /// </summary>
        /// <value>
        /// A read-only collection of EventPipeProvider instances.
        /// </value>
        public static IReadOnlyCollection<EventPipeProvider> Providers =>
            new List<EventPipeProvider>()
            {
                new EventPipeProvider(
                    "Microsoft-Extensions-Logging",
                    EventLevel.LogAlways,
                    263882790666248),
                new EventPipeProvider(
                    "System.Threading.Tasks.TplEventSource",
                    EventLevel.LogAlways,
                    0x80)
            };
    }
}
