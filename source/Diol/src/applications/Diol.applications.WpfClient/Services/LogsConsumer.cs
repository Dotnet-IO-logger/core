using Diol.applications.WpfClient.ViewModels;
using Diol.Core.Consumers;
using Diol.Share.Features;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diol.applications.WpfClient.Services
{
    public class LogsConsumer : IConsumer
    {
        private IEventAggregator eventAggregator;

        public LogsConsumer(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
        }

        public void OnCompleted()
        {
            // IObservable has finished.
            Debug.WriteLine($"{nameof(LogsConsumer)} | {nameof(OnCompleted)}");
        }

        public void OnError(Exception error)
        {
            // write log
            Debug.WriteLine($"{nameof(LogsConsumer)} | {nameof(OnError)}");
        }

        public void OnNext(BaseDto value)
        {
            var json = System.Text.Json.JsonSerializer.Serialize(value, value.GetType());

            Debug.WriteLine($"{nameof(LogsConsumer)} | {nameof(OnNext)}");
            Debug.WriteLine($"{value.CorrelationId} | {value.CategoryName} | {nameof(value.EventName)}");

            this.eventAggregator.GetEvent<LogsEvent>().Publish(json);
        }
    }
}
