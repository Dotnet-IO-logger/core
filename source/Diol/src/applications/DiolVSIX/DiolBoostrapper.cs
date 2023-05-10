using Diol.Core.DotnetProcesses;
using Diol.Wpf.Core.Features.Aspnetcores;
using Diol.Wpf.Core.Features.Https;
using Diol.Wpf.Core.Services;
using Diol.Wpf.Core.Views;
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
        private DiolToolWindow diolToolWindow;

        public DiolBoostrapper(DiolToolWindow diolToolWindow)
        {
            this.diolToolWindow = diolToolWindow;
        }

        protected override DependencyObject CreateShell() 
        {
            var mw = this.Container.Resolve<MainComponent>();

            this.diolToolWindow.Content = mw;

            return mw;
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // register all services here
            containerRegistry.RegisterSingleton<LogsConsumer>();
            containerRegistry.RegisterSingleton<LoggerBuilder>();

            // depends of scenario
            containerRegistry.RegisterSingleton<DotnetProcessesService>();
            // for development we can use LocalDevelopmentProcessProvider
            // for real scenario use another
            containerRegistry.RegisterSingleton<IProcessProvider, LocalDevelopmentProcessProvider>();

            // register http
            containerRegistry.RegisterSingleton<HttpService>();
            containerRegistry.RegisterSingleton<IStore<HttpModel>, HttpStore>();

            // register aspnet
            containerRegistry.RegisterSingleton<AspnetService>();
            containerRegistry.RegisterSingleton<IStore<AspnetcoreModel>, AspnetcoreStore>();
        }
    }
}
