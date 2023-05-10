using Diol.Core.DotnetProcesses;
using Diol.Wpf.Core.Features.Https;
using Diol.Wpf.Core.Features.Shared;
using Diol.Wpf.Core.Services;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Diol.Wpf.Core.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private IProcessProvider dotnetService;
        private LoggerBuilder builder;
        private IEventAggregator eventAggregator;

        public MainWindowViewModel(
            IProcessProvider dotnetService,
            LoggerBuilder builder,
            HttpService httpService,
            IEventAggregator eventAggregator)
        {
            this.dotnetService = dotnetService;
            this.builder = builder;

            this.eventAggregator = eventAggregator;
        }

        public ObservableCollection<HttpViewModel> HttpLogs { get; private set; } =
            new ObservableCollection<HttpViewModel>();

        private bool _canExecute = true;
        public bool CanExecute 
        {
            get => this._canExecute;
            set => SetProperty(ref this._canExecute, value);
        }

        private DelegateCommand _startCommand = null;
        public DelegateCommand StartCommand =>
            _startCommand ?? (_startCommand = new DelegateCommand(StartExecute));

        private void StartExecute()
        {
            var processId = this.dotnetService.GetProcessId();

            if (!processId.HasValue)
            {
                Console.WriteLine($"Process id ({processId}) not found. Please try again");
                return;
            }

            var eventPipeEventSourceWrapper = this.builder
                .Build()
                .SetProcessId(processId.Value)
                .Build();

            Task.Run(() => 
            {
                this.CanExecute = false;
                eventPipeEventSourceWrapper.Start();
                this.CanExecute = true;
            }).ConfigureAwait(false);

            
        }

        private DelegateCommand _clearCommand = null;
        public DelegateCommand ClearCommand =>
            _clearCommand ?? (_clearCommand = new DelegateCommand(ClearExecute));

        private void ClearExecute() 
        {
            this.eventAggregator
                .GetEvent<ClearDataEvent>()
                .Publish(string.Empty);

            this.eventAggregator
                .GetEvent<HttpItemSelectedEvent>()
                .Publish(string.Empty);
        }
    }
}
