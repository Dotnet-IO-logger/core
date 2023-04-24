using Diol.applications.SignalrClient.Hubs;
using Diol.Core.Consumers;
using Diol.Share.Features;
using Microsoft.AspNetCore.SignalR;
using System.Diagnostics;
using System.Text.Json;

namespace Diol.applications.SignalrClient.Consumers
{
    public class SignalRConsumer : IConsumer
    {
        private readonly IHubContext<LogsHub> hubContext;

        public SignalRConsumer(IHubContext<LogsHub> hubContext)
        {
            this.hubContext = hubContext;
        }

        public void OnCompleted()
        {
            // IObservable has finished.
            Debug.WriteLine($"{nameof(SignalRConsumer)} | {nameof(OnCompleted)}");
        }

        public void OnError(Exception error)
        {
            // write log
            Debug.WriteLine($"{nameof(SignalRConsumer)} | {nameof(OnError)}");
        }

        public void OnNext(BaseDto value)
        {
            var json = System.Text.Json.JsonSerializer.Serialize(value, value.GetType());

            Debug.WriteLine($"{nameof(SignalRConsumer)} | {nameof(OnNext)}");
            Debug.WriteLine($"{value.CorrelationId} | {value.CategoryName} | {nameof(value.EventName)}");

            _ = this.hubContext.Clients.All
                .SendAsync(value.CategoryName, json).ConfigureAwait(false);
        }


    }
}
