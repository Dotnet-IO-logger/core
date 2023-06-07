using Diol.Core.DotnetProcesses;
using Diol.Wpf.Core.Features.Https;
using Diol.Wpf.Core.Features.Shared;
using Diol.Wpf.Core.Services;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;

namespace Diol.Wpf.Core.ViewModels
{
    public class MainComponentViewModel : BindableBase
    {
        private IProcessProvider dotnetService;
        private IEventAggregator eventAggregator;
        private IApplicationStateService applicationStateService;
        private LogsSignalrClient logsSignalrClient;
        private IBackendProxy backendProxy;

        public MainComponentViewModel(
            IProcessProvider dotnetService,
            HttpService httpService,
            IEventAggregator eventAggregator,
            IApplicationStateService applicationStateService,
            LogsSignalrClient logsSignalrClient,
            IBackendProxy backendProxy)
        {
            this.dotnetService = dotnetService;

            this.eventAggregator = eventAggregator;
            this.applicationStateService = applicationStateService;

            this.backendProxy = backendProxy;

            this.eventAggregator
                .GetEvent<DebugModeRunnedEvent>()
                .Subscribe(DebugModeRunnedEventHandler, ThreadOption.UIThread);

            this.eventAggregator
                .GetEvent<SignalRConnectionClosedEvent>()
                .Subscribe(SignalRConnectionClosedEventHandler, ThreadOption.UIThread);

            this.eventAggregator
                .GetEvent<ProcessStarted>()
                .Subscribe(ProcessStartedEventHandler, ThreadOption.UIThread);

            this.eventAggregator
                .GetEvent<ProcessFinished>()
                .Subscribe(ProcessFinishedEventHandler, ThreadOption.UIThread);

            this.eventAggregator
                .GetEvent<BackendStarted>()
                .Subscribe(BackendStartedEventHandler, ThreadOption.UIThread);

            this.applicationStateService.Subscribe();

            this.logsSignalrClient = logsSignalrClient;
        }

        private void BackendStartedEventHandler(bool obj)
        {
            this.CanConnectToBe = !obj;
        }

        private void ProcessFinishedEventHandler(int obj)
        {
            this.CanExecute = true;
        }

        private void ProcessStartedEventHandler(int obj)
        {
            this.CanExecute = false;
        }

        private void SignalRConnectionClosedEventHandler()
        {
            this.CanConnectToHub = true;
        }

        public ObservableCollection<HttpViewModel> HttpLogs { get; private set; } =
            new ObservableCollection<HttpViewModel>();

        private bool _canConnectToHub = true;
        public bool CanConnectToHub
        {
            get => this._canConnectToHub;
            set => SetProperty(ref this._canConnectToHub, value);
        }

        private DelegateCommand _connectToHubCommand = null;
        public DelegateCommand ConnectToHubCommand =>
            _connectToHubCommand ?? (_connectToHubCommand = new DelegateCommand(ConnectToHubExecute));

        private void ConnectToHubExecute()
        {
            Task.Run(async () =>
            {
                this.CanConnectToHub = false;
                await this.logsSignalrClient.ConnectAsync()
                    .ConfigureAwait(false);
            });
        }

        private bool _canConnectToBe = true;
        public bool CanConnectToBe
        {
            get => this._canConnectToBe;
            set => SetProperty(ref this._canConnectToBe, value);
        }

        private DelegateCommand _connectToBeCommand = null;
        public DelegateCommand ConnectToBeCommand =>
            _connectToBeCommand ?? (_connectToHubCommand = new DelegateCommand(ConnectToBeExecute));

        private void ConnectToBeExecute()
        {
            this.backendProxy.StartBackend()
                .ConfigureAwait(false);
        }

        private bool _canExecute = true;
        public bool CanExecute
        {
            get => this._canExecute;
            set => SetProperty(ref this._canExecute, value);
        }

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

            Task.Run(async () =>
            {
                //this.CanExecute = false;
                await this.logsSignalrClient
                    .StartProcessing(processId.Value)
                    .ConfigureAwait(false);
            })
            .ContinueWith(t => 
            {
                if (t.IsFaulted) 
                {
                    //this.CanExecute = true;
                }
            }, 
            TaskScheduler.FromCurrentSynchronizationContext());

        }

        private DelegateCommand _clearCommand = null;
        public DelegateCommand ClearCommand =>
            _clearCommand ?? (_clearCommand = new DelegateCommand(ClearExecute));

        private void ClearExecute()
        {
            this.eventAggregator
                .GetEvent<ClearDataEvent>()
                .Publish(string.Empty);

            this.eventAggregator
                .GetEvent<HttpItemSelectedEvent>()
                .Publish(string.Empty);
        }

        private DelegateCommand _settingsCommand = null;
        public DelegateCommand SettingsCommand =>
            _settingsCommand ?? (_settingsCommand = new DelegateCommand(SettingsExecute));

        private void SettingsExecute()
        {
            // for debug purposes
        }

        private void DebugModeRunnedEventHandler(bool obj)
        {
            StartExecute();
        }
    }
}
