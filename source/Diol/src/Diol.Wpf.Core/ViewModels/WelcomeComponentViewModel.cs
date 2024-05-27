using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace Diol.Wpf.Core.ViewModels
{
    /// <summary>
    /// View model for the WelcomeComponent.
    /// </summary>
    public class WelcomeComponentViewModel : BindableBase
    {
        private readonly IRegionManager regionManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="WelcomeComponentViewModel"/> class.
        /// </summary>
        /// <param name="regionManager">The region manager.</param>
        public WelcomeComponentViewModel(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
        }

        #region Status
        private string _status;

        /// <summary>
        /// Gets or sets the status message.
        /// </summary>
        public string StatusMessage
        {
            get { return this._status; }
            set { SetProperty(ref this._status, value); }
        }

        private bool _canGo = true;

        /// <summary>
        /// Gets or sets a value indicating whether the Go command can be executed.
        /// </summary>
        public bool CanGo
        {
            get { return this._canGo; }
            set { SetProperty(ref this._canGo, value); }
        }
        #endregion

        #region GoCommand 
        private DelegateCommand _goCommand;

        /// <summary>
        /// Gets the Go command.
        /// </summary>
        public DelegateCommand GoCommand =>
            this._goCommand ?? (this._goCommand = new DelegateCommand(ExecuteGoCommand));

        private void ExecuteGoCommand()
        {
            this.CanGo = false;

            this.StatusMessage = "Searching...";

            // navigate to main component
            this.regionManager.RequestNavigate("MainRegion", "MainComponent");

            this.CanGo = true;
        }
        #endregion
    }
}
