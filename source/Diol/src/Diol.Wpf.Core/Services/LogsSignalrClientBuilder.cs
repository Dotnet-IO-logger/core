using Diol.Wpf.Core.Features.Aspnetcores;
using Diol.Wpf.Core.Features.EntityFrameworks;
using Diol.Wpf.Core.Features.Https;
using Diol.Wpf.Core.Features.Shared;
using Microsoft.AspNetCore.SignalR.Client;
using Prism.Events;
using System;

namespace Diol.Wpf.Core.Services
{
    public class LogsSignalrClientBuilder : IDisposable
    {
        private HttpService httpService;
        private AspnetService aspnetService;
        private EntityFrameworkService entityFrameworkService;
        private HubConnection connection;
        private readonly IEventAggregator eventAggregator;

        public LogsSignalrClientBuilder(
            HttpService httpService,
            AspnetService aspnetService,
            EntityFrameworkService entityFrameworkService,
            IEventAggregator eventAggregator)
        {
            this.httpService = httpService;
            this.aspnetService = aspnetService;
            this.entityFrameworkService = entityFrameworkService;
            this.eventAggregator = eventAggregator;
        }

        // TODO: move setting to config
        private string uri = "http://localhost:62023/logsHub";

        public LogsSignalrClient BuildClient() 
        {
            this.connection = new HubConnectionBuilder()
                .WithUrl(this.uri)
                .WithAutomaticReconnect()
                .Build();

            return new LogsSignalrClient(
                this.connection, 
                this.eventAggregator,
                this.httpService,
                this.aspnetService,
                this.entityFrameworkService);
        }

        public void Dispose()
        {
            this.connection.DisposeAsync();
        }
    }
}
