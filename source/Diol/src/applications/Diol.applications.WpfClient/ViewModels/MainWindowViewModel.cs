using Diol.applications.WpfClient.Services;
using Diol.Core.DiagnosticClients;
using Diol.Core.DotnetProcesses;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diol.applications.WpfClient.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private DotnetProcessesService dotnetService;
        private LoggerBuilder builder;
        private IEventAggregator eventAggregator;

        public MainWindowViewModel(
            DotnetProcessesService dotnetService,
            LoggerBuilder builder,
            IEventAggregator eventAggregator)
        {
            this.dotnetService = dotnetService;
            this.builder = builder;
            this.eventAggregator = eventAggregator;

            this.eventAggregator.GetEvent<LogsEvent>().Subscribe(ShowLog, ThreadOption.UIThread);
        }


        public ObservableCollection<string> Logs { get; private set; } =
            new ObservableCollection<string>();

        private DelegateCommand _startCommand = null;
        public DelegateCommand StartCommand =>
            _startCommand ?? (_startCommand = new DelegateCommand(StartExecute));

        private void StartExecute()
        {
            // we expect that the process is running (Diol.applications.PlaygroundApi)
            var processName = "Diol.applications.PlaygroundApi";
            var process = this.dotnetService.GetItemOrDefault(processName);

            if (process == null)
            {
                Console.WriteLine($"Process {processName} not found. Please try again");
                return;
            }

            var eventPipeEventSourceWrapper = this.builder
                .Build()
                .SetProcessId(process.Id)
                .Build();

            Task.Run(() => 
            {
                eventPipeEventSourceWrapper.Start();
            }).ConfigureAwait(false);

            
        }

        private DelegateCommand _clearCommand = null;
        public DelegateCommand ClearCommand =>
            _clearCommand ?? (_clearCommand = new DelegateCommand(ClearExecute));

        private void ClearExecute() 
        {
            this.Logs.Clear();
        }

        private void ShowLog(string log) 
        {
            this.Logs.Add(log);
        }
    }
}
