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

        public MainComponentViewModel(
            IProcessProvider dotnetService,
            HttpService httpService,
            IEventAggregator eventAggregator,
            IApplicationStateService applicationStateService,
            LogsSignalrClient logsSignalrClient)
        {
            this.dotnetService = dotnetService;

            this.eventAggregator = eventAggregator;
            this.applicationStateService = applicationStateService;

            this.eventAggregator
                .GetEvent<DebugModeRunnedEvent>()
                .Subscribe(DebugModeRunnedEventHandler, ThreadOption.UIThread);

            this.eventAggregator
                .GetEvent<ProcessStarted>()
                .Subscribe(ProcessStartedEventHandler, ThreadOption.UIThread);

            this.eventAggregator
                .GetEvent<ProcessFinished>()
                .Subscribe(ProcessFinishedEventHandler, ThreadOption.UIThread);

            this.eventAggregator
                .GetEvent<SignalRConnectionEvent>()
                .Subscribe(SignalRConnectionEventHandler, ThreadOption.UIThread);

            this.applicationStateService.Subscribe();

            this.logsSignalrClient = logsSignalrClient;
        }

        private void SignalRConnectionEventHandler(SignalRConnectionEnum status)
        {
            if (status == SignalRConnectionEnum.Connected || status == SignalRConnectionEnum.Reconnected) 
            {

            }
        }

        private void ProcessFinishedEventHandler(int obj)
        {
            this.CanExecute = true;
        }

        private void ProcessStartedEventHandler(int obj)
        {
            this.CanExecute = false;
        }

        public ObservableCollection<HttpViewModel> HttpLogs { get; private set; } =
            new ObservableCollection<HttpViewModel>();

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
