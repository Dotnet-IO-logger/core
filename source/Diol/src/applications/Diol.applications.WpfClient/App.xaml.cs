using Diol.Core.DotnetProcesses;
using Diol.Wpf.Core.Features.Aspnetcores;
using Diol.Wpf.Core.Features.Https;
using Diol.Wpf.Core.Services;
using Diol.Wpf.Core.Views;
using Prism.Ioc;
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
            containerRegistry.RegisterSingleton<LogsConsumer>();
            containerRegistry.RegisterSingleton<LoggerBuilder>();

            // depends of scenario
            containerRegistry.RegisterSingleton<DotnetProcessesService>();
            // for development we can use LocalDevelopmentProcessProvider
            // for real scenario use another
            containerRegistry.RegisterSingleton<IProcessProvider, LocalDevelopmentProcessProvider>();
            containerRegistry.RegisterSingleton<IApplicationStateService, LocalApplicationStateService>();

            // register http
            containerRegistry.RegisterSingleton<HttpService>();
            containerRegistry.RegisterSingleton<IStore<HttpModel>, HttpStore>();

            // register aspnet
            containerRegistry.RegisterSingleton<AspnetService>();
            containerRegistry.RegisterSingleton<IStore<AspnetcoreModel>, AspnetcoreStore>();
        }
    }
}
