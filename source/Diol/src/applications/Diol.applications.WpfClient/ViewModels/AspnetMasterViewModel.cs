using Diol.applications.WpfClient.Features.Aspnetcores;
using Diol.applications.WpfClient.Features.Shared;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diol.applications.WpfClient.ViewModels
{
    public class AspnetMasterViewModel : BindableBase
    {
        private AspnetService service;
        private IEventAggregator eventAggregator;

        public ObservableCollection<AspnetViewModel> AspnetLogs { get; private set; } =
            new ObservableCollection<AspnetViewModel>();

        private AspnetViewModel _selectedItem;
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
