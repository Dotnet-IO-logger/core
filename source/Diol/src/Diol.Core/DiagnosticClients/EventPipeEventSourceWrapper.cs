using Diol.Core.TraceEventProcessors;
using Microsoft.Diagnostics.NETCore.Client;
using Microsoft.Diagnostics.Tracing;

namespace Diol.Core.DiagnosticClients
{
    public class EventPipeEventSourceWrapper : IDisposable
    {
        private readonly EventPipeEventSourceBuilder builder;

        private DiagnosticsClient client;
        private EventPipeSession session;
        private EventPipeEventSource source;

        private TraceEventRouter traceEventRouter;

        public EventPipeEventSourceWrapper(
            EventPipeEventSourceBuilder builder,
            TraceEventRouter traceEventRouter)
        {
            this.traceEventRouter = traceEventRouter;

            this.builder = builder;

            this.client = new DiagnosticsClient(this.builder.ProcessId);
            this.session = this.client.StartEventPipeSession(this.builder.Providers);
            this.source = new EventPipeEventSource(this.session.EventStream);

            this.source.Dynamic.All += Dynamic_All;
        }

        private void Dynamic_All(TraceEvent obj)
        {
            this.traceEventRouter.TraceEvent(obj);
        }

        public void Start()
        {
            try 
            {
                this.source.Process();
            }
            catch (Exception ex) 
            {
                Console.Error.WriteLine(ex.ToString());
            }
        }

        public void Dispose()
        {
            this.source.Dynamic.All -= Dynamic_All;

            this.session?.Dispose();

            this.source?.Dispose();
        }
    }
}
