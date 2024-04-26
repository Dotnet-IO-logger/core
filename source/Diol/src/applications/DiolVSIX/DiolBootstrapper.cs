using Diol.Wpf.Core;
using Diol.Wpf.Core.Services;
using Diol.Wpf.Core.Views;
using DiolVSIX.Services;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Prism.Events;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Unity;
using System.Windows;

namespace DiolVSIX
{
    public class DiolBootstrapper : PrismBootstrapper
    {
        private readonly DiolToolWindowControl diolToolWindow;

        public DiolBootstrapper(
            DiolToolWindowControl diolToolWindow)
        {
            this.diolToolWindow = diolToolWindow;
        }

        protected override DependencyObject CreateShell() 
        {
            var mw = this.Container.Resolve<DiolComponent>();

            this.diolToolWindow.Content = mw;

            return mw;
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            var applicationObject = ServiceProvider.GlobalProvider.GetService(typeof(DTE)) as DTE2;
            var debuggerEvents = applicationObject.Events.DebuggerEvents;

            // register all services here
            containerRegistry.AddDiolWpf<WpfConsumer, VsProcessProvider, VsApplicationStateService>(
                (IContainerProvider container) =>
                {
                    return new VsProcessProvider(applicationObject);
                },
                (IContainerProvider container) => 
                {
                    var eventAggregator = container.Resolve<IEventAggregator>();
                    return new VsApplicationStateService(debuggerEvents, eventAggregator);
                });
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            base.ConfigureModuleCatalog(moduleCatalog);
            moduleCatalog.AddModule<Diol.Wpf.Core.MainModule>();
        }
    }
}
