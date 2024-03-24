using Diol.Share.Services;
using Diol.Wpf.Core;
using Diol.Wpf.Core.Features.Aspnetcores;
using Diol.Wpf.Core.Features.EntityFrameworks;
using Diol.Wpf.Core.Features.Https;
using Diol.Wpf.Core.Services;
using Diol.Wpf.Core.Views;
using DiolVSIX.Services;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Unity;
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
            var mw = this.Container.Resolve<DiolComponent>();

            this.diolToolWindow.Content = mw;

            return mw;
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {

            // register all services here
            containerRegistry.AddDiolWpf<
                WpfConsumer, 
                VsProcessProvider, 
                VsApplicationStateService>();

            // vs specific dependencies
            containerRegistry.RegisterSingleton<RequiredServices>(() => this.requiredServices);

            // depends of scenario
            containerRegistry.RegisterSingleton<DiolExecutor>();
            containerRegistry.RegisterSingleton<DiolBuilder>();
            containerRegistry.RegisterSingleton<WpfConsumer>();
            containerRegistry.RegisterSingleton<DotnetProcessesService>();
            // for development we can use LocalDevelopmentProcessProvider
            // for real scenario use VsProcessProvider
            // containerRegistry.RegisterSingleton<IProcessProvider, LocalDevelopmentProcessProvider>();
            containerRegistry.RegisterSingleton<IProcessProvider, VsProcessProvider>();
            containerRegistry.RegisterSingleton<IApplicationStateService, VsApplicationStateService>();

            // register http
            containerRegistry.RegisterSingleton<HttpService>();
            containerRegistry.RegisterSingleton<IStore<HttpModel>, HttpStore>();

            // register aspnet
            containerRegistry.RegisterSingleton<AspnetService>();
            containerRegistry.RegisterSingleton<IStore<AspnetcoreModel>, AspnetcoreStore>();

            // register entity framework
            containerRegistry.RegisterSingleton<EntityFrameworkService>();
            containerRegistry.RegisterSingleton<IStore<EntityFrameworkModel>, EntityFrameworkStore>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            base.ConfigureModuleCatalog(moduleCatalog);
            moduleCatalog.AddModule<Diol.Wpf.Core.MainModule>();
        }
    }
}
