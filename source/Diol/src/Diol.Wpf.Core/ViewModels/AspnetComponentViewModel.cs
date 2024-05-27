using Diol.Wpf.Core.Features.Aspnetcores;
using Prism.Events;
using Prism.Mvvm;
using System.Windows;

namespace Diol.Wpf.Core.ViewModels
{
    /// <summary>
    /// View model for the ASP.NET component.
    /// </summary>
    public class AspnetComponentViewModel : BindableBase
    {
        private IEventAggregator eventAggregator;

        /// <summary>
        /// Initializes a new instance of the <see cref="AspnetComponentViewModel"/> class.
        /// </summary>
        /// <param name="eventAggregator">The event aggregator.</param>
        public AspnetComponentViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;

            // handle master detail
            this.eventAggregator
                .GetEvent<AspnetItemSelectedEvent>()
                .Subscribe(HandleAspnetItemSelectedEvent, ThreadOption.UIThread);
        }

        private GridLength _detailWidth = new GridLength(0, GridUnitType.Star);

        /// <summary>
        /// Gets or sets the width of the detail.
        /// </summary>
        public GridLength DetailWidth
        {
            get => this._detailWidth;
            set => SetProperty(ref this._detailWidth, value);
        }

        private void HandleAspnetItemSelectedEvent(string obj)
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
