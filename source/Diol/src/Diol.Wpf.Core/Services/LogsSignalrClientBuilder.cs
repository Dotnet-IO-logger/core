using Diol.Wpf.Core.Features.Aspnetcores;
using Diol.Wpf.Core.Features.Https;
using Diol.Wpf.Core.Features.Shared;
using Microsoft.AspNetCore.SignalR.Client;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diol.Wpf.Core.Services
{
    public class LogsSignalrClientBuilder : IDisposable
    {
        private HttpService httpService;
        private AspnetService aspnetService;
        private HubConnection connection;
        private readonly IEventAggregator eventAggregator;

        public LogsSignalrClientBuilder(
            HttpService httpService,
            AspnetService aspnetService,
            IEventAggregator eventAggregator)
        {
            this.httpService = httpService;
            this.aspnetService = aspnetService;
            this.eventAggregator = eventAggregator;
        }

        // TODO: move setting to config
        private string uri = "http://localhost:9200/logsHub";

        public LogsSignalrClient BuildClient() 
        {
            this.connection = new HubConnectionBuilder()
                .WithUrl(this.uri)
                //.WithAutomaticReconnect()
                .Build();

            this.connection.Closed += async (error) =>
            {
                this.eventAggregator
                    .GetEvent<SignalRConnectionClosedEvent>()
                    .Publish();
            };

            return new LogsSignalrClient(
                this.connection, 
                this.eventAggregator,
                this.httpService,
                this.aspnetService);
        }

        public void Dispose()
        {
            this.connection.DisposeAsync();
        }
    }
}
