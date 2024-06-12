using Diol.Wpf.Core.Features.Shared;
using Prism.Events;
using Prism.Mvvm;
using System.Collections.ObjectModel;

namespace Diol.Wpf.Core.ViewModels
{
    /// <summary>
    /// Represents the view model for the diagnostics view.
    /// </summary>
    public class DiagnosticsViewModel : BindableBase
    {
        private readonly IEventAggregator eventAggregator;

        /// <summary>
        /// Gets the collection of diagnostic logs.
        /// </summary>
        public ObservableCollection<DiagnosticModel> Logs { get; private set; } =
            new ObservableCollection<DiagnosticModel>();

        /// <summary>
        /// Initializes a new instance of the <see cref="DiagnosticsViewModel"/> class.
        /// </summary>
        /// <param name="eventAggregator">The event aggregator.</param>
        public DiagnosticsViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;

            this.eventAggregator
                .GetEvent<DiagnosticEvent>()
                .Subscribe(
                    (e) => this.Logs.Add(e),
                    ThreadOption.UIThread);

            this.eventAggregator
                .GetEvent<ClearDataEvent>()
                .Subscribe(
                    (e) => this.Logs.Clear(),
                    ThreadOption.UIThread);
        }
    }
}
