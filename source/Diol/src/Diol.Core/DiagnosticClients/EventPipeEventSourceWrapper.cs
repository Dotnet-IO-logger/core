using Diol.Core.TraceEventProcessors;
using Microsoft.Diagnostics.NETCore.Client;
using Microsoft.Diagnostics.Tracing;
using System;

namespace Diol.Core.DiagnosticClients
{
    public class EventPipeEventSourceWrapper : IDisposable
    {
        private readonly EventPipeEventSourceBuilder builder;

        private TraceEventRouter traceEventRouter;

        private EventPipeEventSource source;

        public EventPipeEventSourceWrapper(
            EventPipeEventSourceBuilder builder,
            TraceEventRouter traceEventRouter)
        {
            this.traceEventRouter = traceEventRouter;

            this.builder = builder;
        }

        public void Start()
        {
            try
            {
                var client = new DiagnosticsClient(this.builder.ProcessId);
                using (var session = client.StartEventPipeSession(this.builder.Providers, false))
                {
                    this.source = new EventPipeEventSource(session.EventStream);

                    this.source.Dynamic.All += this.traceEventRouter.TraceEvent;

                    source.Process();
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

        public void Dispose()
        {
            this.source?.Dispose();
        }
    }
}
