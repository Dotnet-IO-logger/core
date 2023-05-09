using Diol.applications.WpfClient.Features.Https;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Diol.applications.WpfClient.ViewModels
{
    public class HttpMasterDetailViewModel : BindableBase
    {
        private IEventAggregator eventAggregator;

        public HttpMasterDetailViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;

            // handle master detail
            this.eventAggregator
                .GetEvent<HttpItemSelectedEvent>()
                .Subscribe(HandleHttpItemSelectedEvent, ThreadOption.UIThread);
        }

        private GridLength _detailWidth = new GridLength(0, GridUnitType.Star);
        public GridLength DetailWidth
        {
            get => this._detailWidth;
            set => SetProperty(ref this._detailWidth, value);
        }

        private void HandleHttpItemSelectedEvent(string obj)
        {
            if (string.IsNullOrEmpty(obj))
            {
                // handle close
                this.DetailWidth = new GridLength(0, GridUnitType.Star);
            }
            else
            {
                // handle open
                this.DetailWidth = new GridLength(1, GridUnitType.Star);
            }
        }
    }
}
