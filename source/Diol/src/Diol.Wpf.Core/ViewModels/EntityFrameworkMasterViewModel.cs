using Diol.Wpf.Core.Features.EntityFrameworks;
using Diol.Wpf.Core.Features.Shared;
using Prism.Events;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Linq;

namespace Diol.Wpf.Core.ViewModels
{
    /// <summary>
    /// View model for the Entity Framework master view.
    /// </summary>
    internal class EntityFrameworkMasterViewModel : BindableBase
    {
        private EntityFrameworkService service;
        private IEventAggregator eventAggregator;

        /// <summary>
        /// Gets or sets the collection of Entity Framework view models.
        /// </summary>
        public ObservableCollection<EntityFrameworkViewModel> EntityFrameworkLogs { get; private set; } =
            new ObservableCollection<EntityFrameworkViewModel>();

        private EntityFrameworkViewModel _selectedItem;

        /// <summary>
        /// Gets or sets the selected Entity Framework view model.
        /// </summary>
        public EntityFrameworkViewModel SelectedItem
        {
            get => this._selectedItem;
            set
            {
                SetProperty(ref this._selectedItem, value);

                if (this._selectedItem == null)
                {
                    return;
                }

                this.eventAggregator
                    .GetEvent<EntityFrameworkItemSelectedEvent>()
                    .Publish(this._selectedItem.Key);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityFrameworkMasterViewModel"/> class.
        /// </summary>
        /// <param name="service">The Entity Framework service.</param>
        /// <param name="eventAggregator">The event aggregator.</param>
        public EntityFrameworkMasterViewModel(
            EntityFrameworkService service,
            IEventAggregator eventAggregator)
        {
            this.service = service;
            this.eventAggregator = eventAggregator;

            this.eventAggregator
                .GetEvent<EntityFrameworkConnectionStartedEvent>()
                .Subscribe(HandleEntityFrameworkConnectionStartedEvent, ThreadOption.UIThread);

            this.eventAggregator
                .GetEvent<EntityFrameworkConnectionEndedEvent>()
                .Subscribe(HandleEntityFrameworkConnectionEndedEvent, ThreadOption.UIThread);

            this.eventAggregator
                .GetEvent<ClearDataEvent>()
                .Subscribe(HandleClearDataEvent, ThreadOption.UIThread);
        }

        private void HandleClearDataEvent(string obj)
        {
            this.EntityFrameworkLogs.Clear();
        }

        private void HandleEntityFrameworkConnectionEndedEvent(string obj)
        {
            var item = this.service.GetItemOrDefault(obj);

            if (item == null)
            {
                return;
            }

            var vm = this.EntityFrameworkLogs.FirstOrDefault(x => x.Key == item.Key);

            if (vm == null)
            {
                return;
            }

            vm.DurationInMiliSeconds = item?.CommandExecuted?.ElapsedMilliseconds;
        }

        private void HandleEntityFrameworkConnectionStartedEvent(string obj)
        {
            var item = this.service.GetItemOrDefault(obj);

            if (item == null)
            {
                return;
            }

            var vm = new EntityFrameworkViewModel()
            {
                Key = item.Key,
                Database = item?.ConnectionOpening?.Database,
                Server = item?.ConnectionOpening?.Server,
            };

            this.EntityFrameworkLogs.Add(vm);
        }
    }
}
