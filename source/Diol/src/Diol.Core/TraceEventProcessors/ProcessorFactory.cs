using System.Collections.Generic;
using System.Linq;

namespace Diol.Core.TraceEventProcessors
{
    /// <summary>
    /// Represents a factory for creating processors.
    /// </summary>
    public interface IProcessorFactory
    {
        /// <summary>
        /// Gets the collection of processors.
        /// </summary>
        /// <returns>The collection of processors.</returns>
        ICollection<IProcessor> GetProcessors();

        /// <summary>
        /// Gets the processor based on the logger name and event name.
        /// </summary>
        /// <param name="loggerName">The logger name.</param>
        /// <param name="eventName">The event name.</param>
        /// <returns>The processor.</returns>
        IProcessor GetProcessor(string loggerName, string eventName);
    }

    /// <summary>
    /// Represents a factory for creating processors.
    /// </summary>
    public class ProcessorFactory : IProcessorFactory
    {
        private readonly IEnumerable<IProcessor> processors;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessorFactory"/> class.
        /// </summary>
        /// <param name="processors">The collection of processors.</param>
        public ProcessorFactory(IEnumerable<IProcessor> processors)
        {
            this.processors = processors;
        }

        /// <inheritdoc/>
        public ICollection<IProcessor> GetProcessors()
        {
            return this.processors.ToList();
        }

        /// <inheritdoc/>
        public IProcessor GetProcessor(string loggerName, string eventName)
        {
            return this.processors.FirstOrDefault(
                p => p.CheckEvent(loggerName, eventName));
        }
    }
}
