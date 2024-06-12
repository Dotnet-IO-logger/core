using Diol.Core.DiagnosticClients;

namespace Diol.Wpf.Core.Services
{
    /// <summary>
    /// Represents a builder for creating Diol event sources.
    /// </summary>
    public class DiolBuilder
    {
        private readonly EventPipeEventSourceBuilder builder;

        /// <summary>
        /// Initializes a new instance of the <see cref="DiolBuilder"/> class.
        /// </summary>
        /// <param name="builder">The event source builder.</param>
        public DiolBuilder(EventPipeEventSourceBuilder builder)
        {
            this.builder = builder;
        }

        /// <summary>
        /// Gets the event source builder.
        /// </summary>
        /// <returns>The event source builder.</returns>
        public EventPipeEventSourceBuilder GetBuilder()
        {
            return this.builder;
        }
    }
}
