using Diol.Wpf.Core.Features.Shared;
using Prism.Events;

namespace Diol.Wpf.Core.Services
{
    public class DiolExecutor
    {
        private readonly IEventAggregator eventAggregator;
        private readonly DiolBuilder builder;

        public DiolExecutor(
            IEventAggregator eventAggregator,
            DiolBuilder builder)
        {
            this.eventAggregator = eventAggregator;
            this.builder = builder;
        }

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