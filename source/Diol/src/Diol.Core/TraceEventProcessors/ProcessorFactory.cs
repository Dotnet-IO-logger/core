using System.Collections.Generic;
using System.Linq;

namespace Diol.Core.TraceEventProcessors
{
    public interface IProcessorFactory 
    {
        ICollection<IProcessor> GetProcessors();

        IProcessor GetProcessor(string loggerName, string eventName);
    }

    public class ProcessorFactory : IProcessorFactory
    {
        private readonly IEnumerable<IProcessor> processors;

        public ProcessorFactory(IEnumerable<IProcessor> processors)
        {
            this.processors = processors;
        }

        public ICollection<IProcessor> GetProcessors() 
        {
            return this.processors.ToList();
        }

        public IProcessor GetProcessor(string loggerName, string eventName) 
        {
            return this.processors.FirstOrDefault(
                p => p.CheckEvent(loggerName, eventName));
        }
    }
}
