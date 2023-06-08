using Diol.Wpf.Core.Features.Shared;
using Diol.Wpf.Core.Services;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diol.Wpf.Core.ViewModels
{
    public class WelcomeComponentViewModel : BindableBase
    {
        private IEventAggregator eventAggregator;
        private LogsSignalrClient logsSignalrClient;

        public WelcomeComponentViewModel(
            IEventAggregator eventAggregator,
            LogsSignalrClient logsSignalrClient)
        {
            this.eventAggregator = eventAggregator;
            this.logsSignalrClient = logsSignalrClient;

            this.eventAggregator
                .GetEvent<SignalRConnectionEvent>()
                .Subscribe(SignalRConnectionEventHandler, ThreadOption.UIThread);
        }

        private void SignalRConnectionEventHandler(SignalRConnectionEnum status)
        {
            if (status == SignalRConnectionEnum.Connecting)
            {
                this.CanConnected = false;
                this.ErrorMessage = string.Empty;
            }
            else if (status == SignalRConnectionEnum.Connected)
            {
                this.CanConnected = false;
                this.ErrorMessage = string.Empty;
            }
            else if (status == SignalRConnectionEnum.Reconnecting)
            {
                this.CanConnected = false;
                this.ErrorMessage = string.Empty;
            }
            else if (status == SignalRConnectionEnum.Reconnected)
            {
                this.CanConnected = false;
                this.ErrorMessage = string.Empty;
            }
            else if (status == SignalRConnectionEnum.Closed)
            {
                this.CanConnected = true;
                this.ErrorMessage = "Backend connection is closed";
            }
            else if (status == SignalRConnectionEnum.Error)
            {
                this.CanConnected = true;
                this.ErrorMessage = "Backend service is not avaliable. Please check is the backend process is running";
            }
            else
            {
                this.CanConnected = true;
                this.ErrorMessage = "Something is going wrong...";
            }
        }

        private bool _canConnected = true;
        public bool CanConnected
        {
            get => this._canConnected;
            set => SetProperty(ref this._canConnected, value);
        }

        private DelegateCommand _connectCommand = null;
        public DelegateCommand ConnectCommand =>
            _connectCommand ?? (_connectCommand = new DelegateCommand(ConnectExecute));

        private void ConnectExecute()
        {
            Task.Run(async () =>
            {
                await this.logsSignalrClient.ConnectAsync()
                    .ConfigureAwait(false);
            });
        }

        private string _errorMessage = string.Empty;
        public string ErrorMessage
        {
            get => this._errorMessage;
            set => SetProperty(ref this._errorMessage, value);
        }
    }
}
