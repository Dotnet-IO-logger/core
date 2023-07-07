using Diol.Wpf.Core.Features.EntityFrameworks;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Diol.Wpf.Core.ViewModels
{
    public class EntityFrameworkComponentViewModel : BindableBase
    {
        private IEventAggregator eventAggregator;

        public EntityFrameworkComponentViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;

            this.eventAggregator
                .GetEvent<EntityFrameworkItemSelectedEvent>()
                .Subscribe(HandleEntityFrameworkItemSelectedEvent, ThreadOption.UIThread);
        }

        private GridLength _detailWidth = new GridLength(0, GridUnitType.Star);
        public GridLength DetailWidth
        {
            get => this._detailWidth;
            set => SetProperty(ref this._detailWidth, value);
        }

        private void HandleEntityFrameworkItemSelectedEvent(string obj)
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
