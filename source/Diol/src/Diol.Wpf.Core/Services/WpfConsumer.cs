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
    /// <summary>
    /// Represents a WPF consumer that implements the <see cref="IConsumer"/> interface.
    /// </summary>
    public class WpfConsumer : IConsumer
    {
        private readonly IEventAggregator eventAggregator;
        private readonly HttpService httpService;
        private readonly AspnetService aspnetService;
        private readonly EntityFrameworkService entityFrameworkService;

        /// <summary>
        /// Initializes a new instance of the <see cref="WpfConsumer"/> class.
        /// </summary>
        /// <param name="eventAggregator">The event aggregator.</param>
        /// <param name="httpService">The HTTP service.</param>
        /// <param name="aspnetService">The ASP.NET service.</param>
        /// <param name="entityFrameworkService">The Entity Framework service.</param>
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

        /// <summary>
        /// Handles the completion of the consumer.
        /// </summary>
        public void OnCompleted() =>
            Debug.WriteLine($"{nameof(WpfConsumer)} | {nameof(OnCompleted)}");

        /// <summary>
        /// Handles the error occurred in the consumer.
        /// </summary>
        /// <param name="error">The error that occurred.</param>
        public void OnError(Exception error) =>
            Debug.WriteLine($"{nameof(WpfConsumer)} | {nameof(OnError)}");

        /// <summary>
        /// Handles the next value received by the consumer.
        /// </summary>
        /// <param name="value">The value received by the consumer.</param>
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
