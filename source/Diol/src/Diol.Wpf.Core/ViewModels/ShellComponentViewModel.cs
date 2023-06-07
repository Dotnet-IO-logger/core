using Diol.Wpf.Core.Features.Shared;
using Diol.Wpf.Core.Services;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
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
        private IRegionManager regionManager;

        public ShellComponentViewModel(
            IEventAggregator eventAggregator,
            LogsSignalrClient logsSignalrClient,
            IRegionManager regionManager)
        {
            this.eventAggregator = eventAggregator;
            this.logsSignalrClient = logsSignalrClient;
            this.regionManager = regionManager;

            this.eventAggregator
                .GetEvent<SignalRConnectionEvent>()
                .Subscribe(SignalRConnectionEventHandler, ThreadOption.UIThread);

            this.ShowMain(false);
        }

        private void SignalRConnectionEventHandler(SignalRConnectionEnum status)
        {
            if (status == SignalRConnectionEnum.Connected)
            {
                this.ShowMain(true);
            }
            else if (status == SignalRConnectionEnum.Reconnecting)
            {
                this.ShowMain(true);
            }
            else if (status == SignalRConnectionEnum.Reconnected)
            {
                this.ShowMain(true);
            }
            else if (status == SignalRConnectionEnum.Closed)
            {
                this.ShowMain(false);
            }
            else if (status == SignalRConnectionEnum.Error)
            {
                this.ShowMain(false);
            }
            else
            {
                this.ShowMain(false);
            }
        }

        private void ShowMain(bool isShow) 
        {
            if (isShow)
            {
                this.regionManager.RequestNavigate("MainContent", "MainComponent");
            }
            else 
            {
                this.regionManager.RequestNavigate("MainContent", "WelcomeComponent");
            }
        }
    }
}
