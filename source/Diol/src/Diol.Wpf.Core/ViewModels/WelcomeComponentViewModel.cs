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
        private readonly IRegionManager regionManager;

        public WelcomeComponentViewModel(
            IRegionManager regionManager)
        {
            this.regionManager = regionManager;
        }

        #region Status
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
        #endregion

        #region GoCommand 
        private DelegateCommand _goCommand;
        public DelegateCommand GoCommand =>
            this._goCommand ?? (this._goCommand = new DelegateCommand(ExecuteGoCommand));

        private void ExecuteGoCommand()
        {
            this.CanGo = false;

            this.StatusMessage = "Seaching...";

            // navigate to main component
            this.regionManager.RequestNavigate("MainRegion", "MainComponent");

            this.CanGo = true;
        }
        #endregion
    }
}
