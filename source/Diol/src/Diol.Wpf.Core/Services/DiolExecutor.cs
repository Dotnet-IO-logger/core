using Diol.Wpf.Core.Features.Shared;
using Prism.Events;

namespace Diol.Wpf.Core.Services
{
    /// <summary>
    /// Executes the Diol process.
    /// </summary>
    public class DiolExecutor
    {
        private readonly IEventAggregator eventAggregator;
        private readonly DiolBuilder builder;

        /// <summary>
        /// Initializes a new instance of the <see cref="DiolExecutor"/> class.
        /// </summary>
        /// <param name="eventAggregator">The event aggregator.</param>
        /// <param name="builder">The Diol builder.</param>
        public DiolExecutor(
            IEventAggregator eventAggregator,
            DiolBuilder builder)
        {
            this.eventAggregator = eventAggregator;
            this.builder = builder;
        }

        /// <summary>
        /// Starts the Diol processing.
        /// </summary>
        /// <param name="processId">The process ID.</param>
        public void StartProcessing(int processId)
        {
            var executor = this.builder.GetBuilder()
                .SetProcessId(processId)
                .Build();

            // send event start processing
            this.eventAggregator
                .GetEvent<ProcessStarted>()
                .Publish(processId);

            // do processing
            executor.Start();

            // send event finish processing
            this.eventAggregator
                .GetEvent<ProcessFinished>()
                .Publish(processId);
        }
    }
}