using Diol.Share.Features;
using Diol.Share.Features.Aspnetcores;
using Diol.Share.Features.Httpclients;
using Diol.Share.Utils;
using Diol.Wpf.Core.Features.Aspnetcores;
using Diol.Wpf.Core.Features.Https;
using Diol.Wpf.Core.Features.Shared;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Diol.Wpf.Core.Services
{
    public class LogsSignalrClient : IDisposable
    {
        private readonly HubConnection hubConnection;
        private readonly IEventAggregator eventAggregator;
        private HttpService httpService;
        private AspnetService aspnetService;


        public LogsSignalrClient(
            HubConnection hubConnection, 
            IEventAggregator eventAggregator,
            HttpService httpService,
            AspnetService aspnetService)
        {
            this.hubConnection = hubConnection;
            this.eventAggregator = eventAggregator;

            this.httpService = httpService;
            this.aspnetService = aspnetService;

            // setup signalr events
            this.hubConnection.Closed += async (error) => 
            {
                this.UpdateSignalrConnectionStatus(SignalRConnectionEnum.Closed);
            };

            this.hubConnection.Reconnecting += async (error) => 
            {
                this.UpdateSignalrConnectionStatus(SignalRConnectionEnum.Reconnecting);
            };

            this.hubConnection.Reconnected += async (connectionId) =>
            {
                this.UpdateSignalrConnectionStatus(SignalRConnectionEnum.Reconnected);
            };

            this.hubConnection.On<int>(
                "ProcessingStarted",
                (processId) => 
                {
                    this.eventAggregator
                        .GetEvent<ProcessStarted>()
                        .Publish(processId);
                });

            this.hubConnection.On<int>(
                "ProcessingFinished",
                (processId) => 
                {
                    this.eventAggregator
                        .GetEvent<ProcessFinished>()
                        .Publish(processId);
                });

            this.hubConnection.On<ICollection<DotnetProcessInfo>>(
                "ProcessesReceived",
                (processes) =>
                {
                    this.eventAggregator
                        .GetEvent<ProcessesReceivedEvent>()
                        .Publish(processes);
                });

            
            // setup http client
            this.hubConnection.On<string, string, string>("LogReceived", (category, eventName, valueAsJson) => 
            {
                LogReceivedHandler(category, eventName, valueAsJson);
            });
        }

        private void UpdateSignalrConnectionStatus(SignalRConnectionEnum status) 
        {
            this.eventAggregator
                .GetEvent<SignalRConnectionEvent>()
                .Publish(status);
        }

        private void LogReceivedHandler(string categoryName, string eventName, string valueAsJson)
        {
            if (categoryName == "HttpClient" && eventName == "RequestPipelineStartDto")
            {
                var value = JsonConvert.DeserializeObject<RequestPipelineStartDto>(valueAsJson);

                this.httpService.Update(value);
            }
            else if (categoryName == "HttpClient" && eventName == "RequestPipelineRequestHeaderDto")
            {
                var value = JsonConvert.DeserializeObject<RequestPipelineRequestHeaderDto>(valueAsJson);

                this.httpService.Update(value);
            }
            else if (categoryName == "HttpClient" && eventName == "RequestPipelineEndDto")
            {
                var value = JsonConvert.DeserializeObject<RequestPipelineEndDto>(valueAsJson);

                this.httpService.Update(value);
            }
            else if (categoryName == "HttpClient" && eventName == "RequestPipelineResponseHeaderDto")
            {
                var value = JsonConvert.DeserializeObject<RequestPipelineResponseHeaderDto>(valueAsJson);

                this.httpService.Update(value);
            }
            else if (categoryName == "AspnetCore" && eventName == "RequestLogDto")
            {
                var value = JsonConvert.DeserializeObject<RequestLogDto>(valueAsJson);

                this.aspnetService.Update(value);
            }
            else if (categoryName == "AspnetCore" && eventName == "RequestBodyDto")
            {
                var value = JsonConvert.DeserializeObject<RequestBodyDto>(valueAsJson);

                this.aspnetService.Update(value);
            }
            else if (categoryName == "AspnetCore" && eventName == "ResponseLogDto")
            {
                var value = JsonConvert.DeserializeObject<ResponseLogDto>(valueAsJson);

                this.aspnetService.Update(value);
            }
            else if (categoryName == "AspnetCore" && eventName == "ResponseBodyDto")
            {
                var value = JsonConvert.DeserializeObject<ResponseBodyDto>(valueAsJson);

                this.aspnetService.Update(value);
            }
            else
            {
                // log
            }

            var diagnosticData = JsonConvert.DeserializeObject<Dictionary<string, object>>(valueAsJson);

            this.eventAggregator
                .GetEvent<DiagnosticEvent>()
                .Publish(new DiagnosticModel()
                {
                    CategoryName = categoryName,
                    EventName = eventName,
                    ActivityId = diagnosticData["CorrelationId"].ToString()
                });
        }

        public async Task ConnectAsync() 
        {
            try 
            {
                this.UpdateSignalrConnectionStatus(SignalRConnectionEnum.Connecting);

                await this.hubConnection.StartAsync()
                        .ContinueWith(t =>
                        {
                            this.UpdateSignalrConnectionStatus(SignalRConnectionEnum.Connected);
                        });
            }
            catch (Exception ex) 
            {
                this.UpdateSignalrConnectionStatus(SignalRConnectionEnum.Error);
            }
        }

        public async Task GetProcesses() 
        {
            await this.hubConnection
                .InvokeAsync("GetProcesses");
        }

        public async Task StartProcessing(int processId)
        {
            await this.hubConnection
                .InvokeAsync("Subscribe", processId);
        }

        public void Dispose()
        {
            this.hubConnection.StopAsync()
                .ContinueWith(async t => 
                {
                    await this.hubConnection.DisposeAsync()
                        .ConfigureAwait(false);
                });

        }
    }
}
