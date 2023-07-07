using Diol.Share.Features.EntityFrameworks;
using Diol.Wpf.Core.Features.EntityFrameworks;
using Diol.Wpf.Core.Features.Shared;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;

namespace Diol.Wpf.Core.ViewModels
{
    public class EntityFrameworkDetailViewModel : BindableBase
    {
        private EntityFrameworkService service;
        private IEventAggregator eventAggregator;

        private ConnectionOpeningDto _connectionOpening;
        public ConnectionOpeningDto ConnectionOpening
        {
            get => this._connectionOpening;
            set => SetProperty(ref this._connectionOpening, value);
        }

        private CommandExecutingDto _commandExecuting;
        public CommandExecutingDto CommandExecuting
        {
            get => this._commandExecuting;
            set => SetProperty(ref this._commandExecuting, value);
        }

        private CommandExecutedDto _commandExecuted;
        public CommandExecutedDto CommandExecuted
        {
            get => this._commandExecuted;
            set => SetProperty(ref this._commandExecuted, value);
        }

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
