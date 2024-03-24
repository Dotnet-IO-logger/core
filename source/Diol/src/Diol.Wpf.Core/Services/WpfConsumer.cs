using Diol.Share.Consumers;
using Diol.Share.Features;
using Diol.Wpf.Core.Features.Aspnetcores;
using Diol.Wpf.Core.Features.EntityFrameworks;
using Diol.Wpf.Core.Features.Https;
using Diol.Wpf.Core.Features.Shared;
using Prism.Events;
using System;
using System.Diagnostics;

namespace Diol.Wpf.Core.Services
{
    public class WpfConsumer : IConsumer
    {
        private readonly IEventAggregator eventAggregator;
        private readonly HttpService httpService;
        private readonly AspnetService aspnetService;
        private readonly EntityFrameworkService entityFrameworkService;

        public WpfConsumer(
            IEventAggregator eventAggregator, 
            HttpService httpService, 
            AspnetService aspnetService, 
            EntityFrameworkService entityFrameworkService)
        {
            this.eventAggregator = eventAggregator;
            this.httpService = httpService;
            this.aspnetService = aspnetService;
            this.entityFrameworkService = entityFrameworkService;
        }

        public void OnCompleted() =>
            Debug.WriteLine($"{nameof(WpfConsumer)} | {nameof(OnCompleted)}");


        public void OnError(Exception error) =>
            Debug.WriteLine($"{nameof(WpfConsumer)} | {nameof(OnError)}");

        public void OnNext(BaseDto value)
        {
            if (value.CategoryName == "HttpClient")
            {
                this.httpService.Update(value);
            }
            else if (value.CategoryName == "AspnetCore")
            {
                this.aspnetService.Update(value);
            }
            else if (value.CategoryName == "EntityFramework") 
            {
                this.entityFrameworkService.Update(value);
            }
            else
            {
                Debug.WriteLine($"{value.GetType()}");
            }

            // for logging
            this.eventAggregator
                .GetEvent<DiagnosticEvent>()
                .Publish(new DiagnosticModel()
                {
                    CategoryName = value.CategoryName,
                    EventName = value.EventName,
                    ActivityId = value.CorrelationId
                });
        }
    }
}
