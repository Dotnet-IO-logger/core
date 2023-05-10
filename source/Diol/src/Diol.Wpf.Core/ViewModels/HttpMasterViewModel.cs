using Diol.Wpf.Core.Features.Https;
using Diol.Wpf.Core.Features.Shared;
using Prism.Events;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Linq;

namespace Diol.Wpf.Core.ViewModels
{
    public class HttpMasterViewModel : BindableBase
    {
        private HttpService service;
        private IEventAggregator eventAggregator;

        public ObservableCollection<HttpViewModel> HttpLogs { get; private set; } =
            new ObservableCollection<HttpViewModel>();

        private HttpViewModel _selectedItem;
        public HttpViewModel SelectedItem 
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
                    .GetEvent<HttpItemSelectedEvent>()
                    .Publish(this._selectedItem.Key);
            }
        }

        public HttpMasterViewModel(
            HttpService service,
            IEventAggregator eventAggregator)
        {
            this.service = service;
            this.eventAggregator = eventAggregator;

            this.eventAggregator
                .GetEvent<HttpRequestStartedEvent>()
                .Subscribe(HandleHttpRequestStartedEvent, ThreadOption.UIThread);

            this.eventAggregator
                .GetEvent<HttpRequestEndedEvent>()
                .Subscribe(HandleHttpRequestEndedEvent, ThreadOption.UIThread);

            this.eventAggregator
                .GetEvent<ClearDataEvent>()
                .Subscribe(HandleClearDataEvent, ThreadOption.UIThread);
        }

        private void HandleHttpRequestStartedEvent(string obj)
        {
            var item = this.service.GetItemOrDefault(obj);

            if (item == null)
            {
                return;
            }

            var vm = new HttpViewModel()
            {
                Key = item?.Key,
                Uri = item?.Request?.Uri,
                Method = item?.Request?.HttpMethod
            };

            this.HttpLogs.Add(vm);
        }

        private void HandleHttpRequestEndedEvent(string obj)
        {
            var item = this.service.GetItemOrDefault(obj);

            if (item == null)
            {
                return;
            }

            var vm = this.HttpLogs.FirstOrDefault(x => x.Key == obj);

            if (vm == null)
            {
                return;
            }

            vm.ResponseStatusCode = item?.Response?.StatusCode;
            vm.DurationInMiliSeconds = item?.Response?.ElapsedMilliseconds;
        }

        private void HandleClearDataEvent(string obj) 
        {
            this.HttpLogs.Clear();
        }
    }
}
