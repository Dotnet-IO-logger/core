using Diol.Wpf.Core.Features.EntityFrameworks;
using Prism.Events;
using Prism.Mvvm;
using System.Windows;

namespace Diol.Wpf.Core.ViewModels
{
    /// <summary>
    /// Represents the view model for the EntityFrameworkComponent.
    /// </summary>
    public class EntityFrameworkComponentViewModel : BindableBase
    {
        private IEventAggregator eventAggregator;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityFrameworkComponentViewModel"/> class.
        /// </summary>
        /// <param name="eventAggregator">The event aggregator.</param>
        public EntityFrameworkComponentViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;

            this.eventAggregator
                .GetEvent<EntityFrameworkItemSelectedEvent>()
                .Subscribe(HandleEntityFrameworkItemSelectedEvent, ThreadOption.UIThread);
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
