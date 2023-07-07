using Diol.Share.Services;
using Diol.Wpf.Core.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diol.Wpf.Core.ViewModels
{
    public class WelcomeComponentViewModel : BindableBase
    {
        private readonly DotnetProcessesService dotnetService;
        private readonly IRegionManager regionManager;
        private readonly LogsSignalrClient logsSignalrClient;
        const string diolBackendServiceName = "DiolBackendService";

        public WelcomeComponentViewModel(
            DotnetProcessesService dotnetService,
            IRegionManager regionManager,
            LogsSignalrClient logsSignalrClient)
        {
            this.dotnetService = dotnetService;
            this.regionManager = regionManager;
            this.logsSignalrClient = logsSignalrClient;
        }

        private string _status;
        public string StatusMessage 
        {
            get { return this._status; }
            set { SetProperty(ref this._status, value); }
        }

        private bool _canGo = true;
        public bool CanGo
        {
            get { return this._canGo; }
            set { SetProperty(ref this._canGo, value); }
        }


        private DelegateCommand _goCommand;
        public DelegateCommand GoCommand =>
            this._goCommand ?? (this._goCommand = new DelegateCommand(ExecuteGoCommand));

        private void ExecuteGoCommand()
        {
            this.CanGo = false;

            this.StatusMessage = "Seaching...";

            var diolBackendService = this.dotnetService.GetItemOrDefault(diolBackendServiceName);

            if (diolBackendService != null)
            {
                this.StatusMessage = "DiolBackendService is avaliable";

                TryToConnect();

                // navigate to main component
                this.regionManager.RequestNavigate("MainRegion", "MainComponent");
            }
            else
            {
                this.StatusMessage = "DiolBackendService is not avaliable. Please run";
            }

            this.CanGo = true;
        }

        private void TryToConnect()
        {
            Task.Run(async () =>
            {
                await this.logsSignalrClient.ConnectAsync();
            });
        }
    }
}
