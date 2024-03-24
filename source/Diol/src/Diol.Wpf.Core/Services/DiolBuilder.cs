using Diol.Core.DiagnosticClients;

namespace Diol.Wpf.Core.Services
{
    public class DiolBuilder
    {
        private readonly EventPipeEventSourceBuilder builder;

        public DiolBuilder(EventPipeEventSourceBuilder builder)
        {
            this.builder = builder;
        }

        public EventPipeEventSourceBuilder GetBuilder()
        {
            return this.builder;
        }
    }
}
