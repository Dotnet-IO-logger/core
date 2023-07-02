using Diol.Aspnet.Hubs;
using Diol.Core.Consumers;
using Diol.Share.Features;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Text.Json;

namespace Diol.Aspnet.Consumers
{
    public class SignalRConsumer : IConsumer
    {
        private readonly IHubContext<LogsHub> hubContext;
        private readonly ILogger<SignalRConsumer> logger;

        public SignalRConsumer(
            IHubContext<LogsHub> hubContext,
            ILogger<SignalRConsumer> logger)
        {
            this.hubContext = hubContext;
            this.logger = logger;
        }

        public void OnCompleted()
        {
            // IObservable has finished.
            this.logger.LogInformation(
                "{className} | {methodName}", 
                nameof(SignalRConsumer), 
                nameof(OnCompleted));
        }

        public void OnError(Exception error)
        {
            // write log
            this.logger.LogError(
                "{className} | {methodName}",
                nameof(SignalRConsumer),
                nameof(OnError));
        }

        public void OnNext(BaseDto value)
        {
            var valueAsJson = JsonSerializer.Serialize(
                value,
                value.GetType());

            this.logger.LogInformation(
                "{className} | {methodName}",
                nameof(SignalRConsumer),
                nameof(OnNext));

            this.logger.LogInformation(
                "{correlationId} | {categoryName} | {eventName}",
                value.CorrelationId,
                value.CategoryName,
                value.EventName);

            var sendTask = this.hubContext.Clients
                .Group(value.ProcessId.ToString())
                .SendAsync("LogReceived", value.CategoryName, value.EventName, valueAsJson);

            Task.WaitAll(sendTask);
        }
    }
}
