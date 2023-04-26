using Diol.applications.WpfClient.Features.Https;
using Diol.applications.WpfClient.Services;
using Diol.Core.DiagnosticClients;
using Diol.Core.DotnetProcesses;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diol.applications.WpfClient.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private IProcessInfoProvider dotnetService;
        private LoggerBuilder builder;
        private HttpService httpService;
        private IEventAggregator eventAggregator;

        public MainWindowViewModel(
            IProcessInfoProvider dotnetService,
            LoggerBuilder builder,
            HttpService httpService,
            IEventAggregator eventAggregator)
        {
            this.dotnetService = dotnetService;
            this.builder = builder;
            this.httpService = httpService;

            this.eventAggregator = eventAggregator;

            this.eventAggregator
                .GetEvent<HttpRequestStartedEvent>()
                .Subscribe(HandleHttpRequestStartedEvent, ThreadOption.UIThread);

            this.eventAggregator
                .GetEvent<HttpRequestEndedEvent>()
                .Subscribe(HandleHttpRequestEndedEvent, ThreadOption.UIThread);
        }

        private void HandleHttpRequestStartedEvent(string obj)
        {
            var item = this.httpService.GetItemOrDefault(obj);

            if (item == null) 
            {
                return;
            }

            var vm = new HttpViewModel() 
            {
                Key = item?.Key,
                Uri = item?.Request?.Uri,
                Method = item?.Request?.HttpMethod
            };

            this.HttpLogs.Add(vm);
        }

        private void HandleHttpRequestEndedEvent(string obj)
        {
            var item = this.httpService.GetItemOrDefault(obj);

            if (item == null)
            {
                return;
            }

            var vm = this.HttpLogs.FirstOrDefault(x => x.Key == obj);

            if(vm == null) 
            {
                return;
            }

            vm.ResponseStatusCode = item?.Response?.StatusCode;
            vm.DurationInMiliSeconds = item?.Response?.ElapsedMilliseconds;
        }

        public ObservableCollection<HttpViewModel> HttpLogs { get; private set; } =
            new ObservableCollection<HttpViewModel>();

        private DelegateCommand _startCommand = null;
        public DelegateCommand StartCommand =>
            _startCommand ?? (_startCommand = new DelegateCommand(StartExecute));

        private void StartExecute()
        {
            var processId = this.dotnetService.GetProcessId();

            if (!processId.HasValue)
            {
                Console.WriteLine($"Process id ({processId}) not found. Please try again");
                return;
            }

            var eventPipeEventSourceWrapper = this.builder
                .Build()
                .SetProcessId(processId.Value)
                .Build();

            Task.Run(() => 
            {
                eventPipeEventSourceWrapper.Start();
            }).ConfigureAwait(false);

            
        }

        private DelegateCommand _clearCommand = null;
        public DelegateCommand ClearCommand =>
            _clearCommand ?? (_clearCommand = new DelegateCommand(ClearExecute));

        private void ClearExecute() 
        {
            this.HttpLogs.Clear();
        }
    }
}
