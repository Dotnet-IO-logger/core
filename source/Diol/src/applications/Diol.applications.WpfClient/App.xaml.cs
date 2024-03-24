using Diol.Share.Services;
using Diol.Wpf.Core;
using Diol.Wpf.Core.Services;
using Diol.Wpf.Core.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Unity;
using System.Windows;

namespace Diol.applications.WpfClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            var w = Container.Resolve<MainWindow>();
            return w;
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // register all services here
            containerRegistry.AddDiolWpf<
                WpfConsumer, 
                LocalDevelopmentProcessProvider, 
                LocalApplicationStateService>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            base.ConfigureModuleCatalog(moduleCatalog);
            moduleCatalog.AddModule<Diol.Wpf.Core.MainModule>();
        }
    }
}
