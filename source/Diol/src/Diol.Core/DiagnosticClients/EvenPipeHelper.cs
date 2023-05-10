using Microsoft.Diagnostics.NETCore.Client;
using System.Collections.Generic;
using System.Diagnostics.Tracing;

namespace Diol.Core.DiagnosticClients
{
    public static class EvenPipeHelper
    {
        public static IReadOnlyCollection<EventPipeProvider> Providers =>
            new List<EventPipeProvider>()
            {
                // Required providers:
                // 1. Microsoft-Extensions-Logging
                // 2. System.Threading.Tasks.TplEventSource
                // 263882790666248 -> for json result
                new EventPipeProvider(
                    "Microsoft-Extensions-Logging",
                    EventLevel.LogAlways,
                    263882790666248),
                new EventPipeProvider(
                    "System.Threading.Tasks.TplEventSource",
                    EventLevel.LogAlways,
                    0x80),
                // after last check, looks like we don't need this provider
                //new EventPipeProvider(
                //    "System-Net-Http",
                //    EventLevel.LogAlways,
                //    long.MaxValue),
                //new EventPipeProvider(
                //    "Microsoft.AspNetCore.HttpLogging",
                //    EventLevel.LogAlways,
                //    long.MaxValue),
            };
    }
}
