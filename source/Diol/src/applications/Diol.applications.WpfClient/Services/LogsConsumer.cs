using Diol.applications.WpfClient.Features.Aspnetcores;
using Diol.applications.WpfClient.Features.Https;
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
        private HttpService httpService;
        private AspnetService aspnetService;

        public LogsConsumer(
            HttpService httpService,
            AspnetService aspnetService)
        {
            this.httpService = httpService;
            this.aspnetService = aspnetService;
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
