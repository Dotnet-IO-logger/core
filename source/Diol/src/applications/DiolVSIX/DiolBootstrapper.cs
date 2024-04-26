using Diol.Wpf.Core;
using Diol.Wpf.Core.Services;
using Diol.Wpf.Core.Views;
using DiolVSIX.Services;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Unity;
using System.Windows;

namespace DiolVSIX
{
    public class DiolBootstrapper : PrismBootstrapper
    {
        private readonly DiolToolWindowControl diolToolWindow;
        private readonly RequiredServices requiredServices;

        public DiolBootstrapper(
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
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            base.ConfigureModuleCatalog(moduleCatalog);
            moduleCatalog.AddModule<Diol.Wpf.Core.MainModule>();
        }
    }
}
