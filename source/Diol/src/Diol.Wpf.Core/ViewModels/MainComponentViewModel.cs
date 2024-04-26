using Diol.Share.Services;
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
        private DiolExecutor diolExecutor;

        public MainComponentViewModel(
            IProcessProvider dotnetService,
            IEventAggregator eventAggregator,
            IApplicationStateService applicationStateService,
            DiolExecutor diolExecutor)
        {
            this.diolExecutor = diolExecutor;
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

            this.applicationStateService.Subscribe();

            CanProcess(true);

            this.DebugMenuItemVisibility = Visibility.Hidden;

#if DEBUG
            this.DebugMenuItemVisibility = Visibility.Visible;
#endif
        }

        private void CanProcess(bool isConnected) 
        {
            if (isConnected)
            {
                this.CanExecute = true;
            }
            else 
            {
                this.CanExecute = false;
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

        #region Execute Command

        private Visibility _debugMenuItemVisibility;

        public Visibility DebugMenuItemVisibility
        {
            get => _debugMenuItemVisibility;
            set => SetProperty(ref this._debugMenuItemVisibility, value);
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

            Task.Run(() =>
            {
                this.diolExecutor.StartProcessing(processId.Value);
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
        #endregion

        #region Clear Command
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
        #endregion

        #region Settings
        private DelegateCommand _settingsCommand = null;
        public DelegateCommand SettingsCommand =>
            _settingsCommand ?? (_settingsCommand = new DelegateCommand(SettingsExecute));

        private void SettingsExecute()
        {
            // for debug purposes
        }
        #endregion

        private void DebugModeRunnedEventHandler(bool obj)
        {
            StartExecute();
        }
    }
}
