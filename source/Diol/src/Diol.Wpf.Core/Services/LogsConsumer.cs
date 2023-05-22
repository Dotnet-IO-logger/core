using Diol.Core.Consumers;
using Diol.Share.Features;
using Diol.Wpf.Core.Features.Aspnetcores;
using Diol.Wpf.Core.Features.Https;
using Diol.Wpf.Core.Features.Shared;
using Newtonsoft.Json;
using Prism.Events;
using System;
using System.Diagnostics;

namespace Diol.Wpf.Core.Services
{
    public class LogsConsumer : IConsumer
    {
        private HttpService httpService;
        private AspnetService aspnetService;
        private readonly IEventAggregator eventAggregator;

        public LogsConsumer(
            HttpService httpService,
            AspnetService aspnetService,
            IEventAggregator eventAggregator)
        {
            this.httpService = httpService;
            this.aspnetService = aspnetService;
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
            var json = JsonConvert.SerializeObject(value);

            Debug.WriteLine($"{nameof(LogsConsumer)} | {nameof(OnNext)}");
            Debug.WriteLine($"{value.CorrelationId} | {value.CategoryName} | {value.EventName}");

            // display logs infor
            this.eventAggregator
                .GetEvent<DiagnosticEvent>()
                .Publish(new DiagnosticModel() 
                {
                    ActivityId = value.CorrelationId,
                    EventName = value.EventName,
                    CategoryName = value.CategoryName
                });

            if (value.CategoryName == "HttpClient")
            {
                this.httpService.Update(value);
            }
            else if (value.CategoryName == "AspnetCore") 
            {
                this.aspnetService.Update(value);
            }
        }
    }
}
