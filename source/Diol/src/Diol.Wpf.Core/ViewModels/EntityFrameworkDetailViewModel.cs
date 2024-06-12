using Diol.Share.Features.EntityFrameworks;
using Diol.Wpf.Core.Features.EntityFrameworks;
using Diol.Wpf.Core.Features.Shared;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;

namespace Diol.Wpf.Core.ViewModels
{
    /// <summary>
    /// View model for the Entity Framework detail view.
    /// </summary>
    public class EntityFrameworkDetailViewModel : BindableBase
    {
        private EntityFrameworkService service;
        private IEventAggregator eventAggregator;

        private ConnectionOpeningDto _connectionOpening;
        /// <summary>
        /// Gets or sets the connection opening details.
        /// </summary>
        public ConnectionOpeningDto ConnectionOpening
        {
            get => this._connectionOpening;
            set => SetProperty(ref this._connectionOpening, value);
        }

        private CommandExecutingDto _commandExecuting;
        /// <summary>
        /// Gets or sets the command executing details.
        /// </summary>
        public CommandExecutingDto CommandExecuting
        {
            get => this._commandExecuting;
            set => SetProperty(ref this._commandExecuting, value);
        }

        private CommandExecutedDto _commandExecuted;
        /// <summary>
        /// Gets or sets the command executed details.
        /// </summary>
        public CommandExecutedDto CommandExecuted
        {
            get => this._commandExecuted;
            set => SetProperty(ref this._commandExecuted, value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityFrameworkDetailViewModel"/> class.
        /// </summary>
        /// <param name="service">The Entity Framework service.</param>
        /// <param name="eventAggregator">The event aggregator.</param>
        public EntityFrameworkDetailViewModel(
            EntityFrameworkService service,
            IEventAggregator eventAggregator)
        {
            this.service = service;
            this.eventAggregator = eventAggregator;

            this.eventAggregator
                .GetEvent<EntityFrameworkItemSelectedEvent>()
                .Subscribe(HandleEntityFrameworkItemSelectedEvent, ThreadOption.UIThread);

            this.eventAggregator
                .GetEvent<ClearDataEvent>()
                .Subscribe(HandleClearDataEvent, ThreadOption.UIThread);
        }

        private DelegateCommand _closeCommand = null;
        /// <summary>
        /// Gets the command to close the view.
        /// </summary>
        public DelegateCommand CloseCommand =>
            _closeCommand ?? (_closeCommand = new DelegateCommand(CloseExecute));

        private void CloseExecute()
        {
            this.eventAggregator
                .GetEvent<EntityFrameworkItemSelectedEvent>()
                .Publish(string.Empty);
        }

        private void HandleClearDataEvent(string obj)
        {
            this.ConnectionOpening = null;
            this.CommandExecuting = null;
            this.CommandExecuted = null;
        }

        private void HandleEntityFrameworkItemSelectedEvent(string obj)
        {
            this.HandleClearDataEvent(obj);

            var item = this.service.GetItemOrDefault(obj);

            if (item == null)
            {
                return;
            }

            this.ConnectionOpening = item.ConnectionOpening;
            this.CommandExecuting = item.CommandExecuting;
            this.CommandExecuted = item.CommandExecuted;
        }
    }
}
