using Diol.Core.DotnetProcesses;
using Diol.Wpf.Core.Features.Aspnetcores;
using Diol.Wpf.Core.Features.Https;
using Diol.Wpf.Core.Services;
using Diol.Wpf.Core.Views;
using DiolVSIX.Services;
using Prism.Ioc;
using Prism.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DiolVSIX
{
    public class DiolBoostrapper : PrismBootstrapper
    {
        private readonly DiolToolWindowControl diolToolWindow;
        private readonly RequiredServices requiredServices;

        public DiolBoostrapper(
            DiolToolWindowControl diolToolWindow,
            RequiredServices requiredServices)
        {
            this.diolToolWindow = diolToolWindow;
            this.requiredServices = requiredServices;
        }

        protected override DependencyObject CreateShell() 
        {
            var mw = this.Container.Resolve<ShellComponent>();

            this.diolToolWindow.Content = mw;

            return mw;
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // register all services here
            containerRegistry.RegisterSingleton<LogsSignalrClientBuilder>();
            containerRegistry.RegisterSingleton<LogsSignalrClient>(container =>
            {
                var builder = container.Resolve<LogsSignalrClientBuilder>();
                return builder.BuildClient();
            });

            // depends of scenario
            containerRegistry.RegisterSingleton<DotnetProcessesService>();
            // for development we can use LocalDevelopmentProcessProvider
            // for real scenario use VsProcessProvider
            //containerRegistry.RegisterSingleton<IProcessProvider, LocalDevelopmentProcessProvider>();
            containerRegistry.RegisterSingleton<RequiredServices>(() => this.requiredServices);
            containerRegistry.RegisterSingleton<IProcessProvider, VsProcessProvider>();
            containerRegistry.RegisterSingleton<IApplicationStateService, VsApplicationStateService>();

            // register http
            containerRegistry.RegisterSingleton<HttpService>();
            containerRegistry.RegisterSingleton<IStore<HttpModel>, HttpStore>();

            // register aspnet
            containerRegistry.RegisterSingleton<AspnetService>();
            containerRegistry.RegisterSingleton<IStore<AspnetcoreModel>, AspnetcoreStore>();
        }
    }
}
