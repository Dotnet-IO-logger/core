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
using System.Windows;

namespace Diol.Wpf.Core.ViewModels
{
    public class ShellComponentViewModel : BindableBase
    {
        private IEventAggregator eventAggregator;
        private LogsSignalrClient logsSignalrClient;

        public ShellComponentViewModel(
            IEventAggregator eventAggregator,
            LogsSignalrClient logsSignalrClient)
        {
            this.eventAggregator = eventAggregator;
            this.logsSignalrClient = logsSignalrClient;

            this.eventAggregator
                .GetEvent<SignalRConnectionEvent>()
                .Subscribe(SignalRConnectionEventHandler, ThreadOption.UIThread);

            this.ShowMain(false);
        }

        private void SignalRConnectionEventHandler(SignalRConnectionEnum status)
        {
            if (status == SignalRConnectionEnum.Connected)
            {
                this.IsConnected = false;
                this.ErrorMessage = "Connected to the backend service!";
                this.ShowMain(true);
            }
            else if (status == SignalRConnectionEnum.Reconnecting)
            {
                this.IsConnected = false;
                this.ErrorMessage = "Reconnecting to the backend...";
                this.ShowMain(true);
            }
            else if (status == SignalRConnectionEnum.Reconnected)
            {
                this.IsConnected = false;
                this.ErrorMessage = "Reconneced to the backend service!";
                this.ShowMain(true);
            }
            else if (status == SignalRConnectionEnum.Closed)
            {
                this.IsConnected = true;
                this.ErrorMessage = "Reconneced to the backend service!";
                this.ShowMain(false);
            }
            else if (status == SignalRConnectionEnum.Error)
            {
                this.IsConnected = true;
                // need to notify user that the backend service is not avaliable
                this.ErrorMessage = "Backend service is not avaliable";
                this.ShowMain(false);
            }
            else
            {
                this.ErrorMessage = "Something is going wrong...";
                this.ShowMain(false);
            }
        }

        private void ShowMain(bool isShow) 
        {
            if (isShow)
            {
                this.MainComponentVisibility = Visibility.Visible;
                this.ShellComponentVisibility = Visibility.Hidden;
            }
            else 
            {
                this.MainComponentVisibility = Visibility.Hidden;
                this.ShellComponentVisibility = Visibility.Visible;
            }
        }

        private Visibility _mainComponentVisibility;
        public Visibility MainComponentVisibility
        {
            get => this._mainComponentVisibility;
            set => SetProperty(ref this._mainComponentVisibility, value);
        }

        private Visibility _shellComponentVisibility;
        public Visibility ShellComponentVisibility
        {
            get => this._shellComponentVisibility;
            set => SetProperty(ref this._shellComponentVisibility, value);
        }

        private bool _isConnected = true;
        public bool IsConnected
        {
            get => this._isConnected;
            set => SetProperty(ref this._isConnected, value);
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
