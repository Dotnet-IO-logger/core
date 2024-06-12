using Diol.Core.TraceEventProcessors;
using Microsoft.Diagnostics.NETCore.Client;
using Microsoft.Diagnostics.Tracing;
using System;

namespace Diol.Core.DiagnosticClients
{
    /// <summary>
    /// Wrapper class for EventPipeEventSource that provides functionality to start and stop event tracing.
    /// </summary>
    public class EventPipeEventSourceWrapper : IDisposable
    {
        private readonly EventPipeEventSourceBuilder builder;
        private readonly TraceEventRouter traceEventRouter;
        private EventPipeEventSource source;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventPipeEventSourceWrapper"/> class.
        /// </summary>
        /// <param name="builder">The EventPipeEventSourceBuilder instance.</param>
        /// <param name="traceEventRouter">The TraceEventRouter instance.</param>
        public EventPipeEventSourceWrapper(
            EventPipeEventSourceBuilder builder,
            TraceEventRouter traceEventRouter)
        {
            this.traceEventRouter = traceEventRouter;
            this.builder = builder;
        }

        /// <summary>
        /// Starts the event tracing.
        /// </summary>
        public void Start()
        {
            try
            {
                var client = new DiagnosticsClient(this.builder.ProcessId);
                using (var session = client.StartEventPipeSession(this.builder.Providers, false))
                {
                    this.source = new EventPipeEventSource(session.EventStream);

                    this.source.Dynamic.All += this.traceEventRouter.TraceEvent;

                    this.source.Process();
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.ToString());
            }
            finally
            {
                // Unsubscribe from the event
                if (this.source != null)
                {
                    this.source.Dynamic.All -= this.traceEventRouter.TraceEvent;
                    this.source.Dispose();
                }
            }
        }

        /// <summary>
        /// Stops the event tracing.
        /// </summary>
        public void Stop()
        {
            try
            {
                this.source.StopProcessing();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        /// Disposes the EventPipeEventSource instance.
        /// </summary>
        public void Dispose()
        {
            this.source?.Dispose();
        }
    }
}
