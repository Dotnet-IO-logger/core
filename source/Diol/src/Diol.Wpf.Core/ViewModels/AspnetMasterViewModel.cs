using Diol.Wpf.Core.Features.Aspnetcores;
using Diol.Wpf.Core.Features.Shared;
using Prism.Events;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Linq;

namespace Diol.Wpf.Core.ViewModels
{
    /// <summary>
    /// View model for the AspnetMasterView.
    /// </summary>
    public class AspnetMasterViewModel : BindableBase
    {
        private AspnetService service;
        private IEventAggregator eventAggregator;

        /// <summary>
        /// Gets or sets the collection of AspnetViewModels.
        /// </summary>
        public ObservableCollection<AspnetViewModel> AspnetLogs { get; private set; } =
            new ObservableCollection<AspnetViewModel>();

        private AspnetViewModel _selectedItem;

        /// <summary>
        /// Gets or sets the selected AspnetViewModel.
        /// </summary>
        public AspnetViewModel SelectedItem
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
                    .GetEvent<AspnetItemSelectedEvent>()
                    .Publish(this._selectedItem.Key);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AspnetMasterViewModel"/> class.
        /// </summary>
        /// <param name="service">The AspnetService.</param>
        /// <param name="eventAggregator">The event aggregator.</param>
        public AspnetMasterViewModel(
            AspnetService service,
            IEventAggregator eventAggregator)
        {
            this.service = service;
            this.eventAggregator = eventAggregator;

            this.eventAggregator
                .GetEvent<AspnetRequestStartedEvent>()
                .Subscribe(HandleAspnetRequestStartedEvent, ThreadOption.UIThread);

            this.eventAggregator
                .GetEvent<AspnetRequestEndedEvent>()
                .Subscribe(HandleAspnetRequestEndedEvent, ThreadOption.UIThread);

            this.eventAggregator
                .GetEvent<ClearDataEvent>()
                .Subscribe(HandleClearDataEvent, ThreadOption.UIThread);
        }

        private void HandleAspnetRequestStartedEvent(string obj)
        {
            var item = this.service.GetItemOrDefault(obj);

            if (item == null)
            {
                return;
            }

            var vm = new AspnetViewModel()
            {
                Key = item?.Key,
                Method = item?.Request?.Method,
                Uri = $"{item.Request.Scheme}://{item.Request.Host}{item.Request.Path}"
            };

            this.AspnetLogs.Add(vm);
        }

        private void HandleAspnetRequestEndedEvent(string obj)
        {
            var item = this.service.GetItemOrDefault(obj);

            if (item == null)
            {
                return;
            }

            var vm = this.AspnetLogs.FirstOrDefault(x => x.Key == item.Key);

            if (vm == null)
            {
                return;
            }

            vm.ResponseStatusCode = item?.Response?.StatusCode;
        }

        private void HandleClearDataEvent(string obj)
        {
            this.AspnetLogs.Clear();
        }
    }
}
