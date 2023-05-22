using Diol.Wpf.Core.Features.Shared;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diol.Wpf.Core.ViewModels
{
    public class DiagnosticsViewModel : BindableBase
    {
        private readonly IEventAggregator eventAggregator;

        public ObservableCollection<DiagnosticModel> Logs { get; private set; } =
            new ObservableCollection<DiagnosticModel>();

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
