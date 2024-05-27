using Diol.Aspnet.Hubs;
using Diol.Share.Consumers;
using Diol.Share.Features;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Diol.Aspnet.Consumers
{
    /// <summary>
    /// Represents a consumer that sends log messages to SignalR clients.
    /// </summary>
    public class SignalRConsumer : IConsumer
    {
        private readonly IHubContext<LogsHub> hubContext;
        private readonly ILogger<SignalRConsumer> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="SignalRConsumer"/> class.
        /// </summary>
        /// <param name="hubContext">The SignalR hub context.</param>
        /// <param name="logger">The logger.</param>
        public SignalRConsumer(
            IHubContext<LogsHub> hubContext,
            ILogger<SignalRConsumer> logger)
        {
            this.hubContext = hubContext;
            this.logger = logger;
        }

        /// <summary>
        /// Called when the IObservable has completed.
        /// </summary>
        public void OnCompleted()
        {
            // IObservable has finished.
            this.logger.LogInformation(
                "{className} | {methodName}",
                nameof(SignalRConsumer),
                nameof(OnCompleted));
        }

        /// <summary>
        /// Called when an error occurs in the IObservable.
        /// </summary>
        /// <param name="error">The error that occurred.</param>
        public void OnError(Exception error)
        {
            // write log
            this.logger.LogError(
                "{className} | {methodName}",
                nameof(SignalRConsumer),
                nameof(OnError));
        }

        /// <summary>
        /// Called when a new log message is received.
        /// </summary>
        /// <param name="value">The log message.</param>
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
