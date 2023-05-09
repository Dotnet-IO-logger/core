using Diol.applications.WpfClient.Services;
using Diol.Core.Consumers;
using Diol.Core.DiagnosticClients;
using Diol.Core.DotnetProcesses;
using Diol.Core.TraceEventProcessors;
using Diol.Share.Features.Aspnetcores;
using Diol.Share.Features.Httpclients;
using Diol.Share.Features;
using Prism.Ioc;
using Prism.Unity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Diol.applications.WpfClient.Features.Https;
using Diol.applications.WpfClient.Features.Aspnetcores;

namespace Diol.applications.WpfClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            var w = Container.Resolve<Views.MainWindow>();
            return w;
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // register all services here
            containerRegistry.RegisterSingleton<LogsConsumer>();
            containerRegistry.RegisterSingleton<LoggerBuilder>();

            // depends of scenario
            containerRegistry.RegisterSingleton<DotnetProcessesService>();
            containerRegistry.RegisterSingleton<IProcessInfoProvider, LocalDevelopmentProcessInfoProvider>();

            // register http
            containerRegistry.RegisterSingleton<HttpService>();
            containerRegistry.RegisterSingleton<IStore<HttpModel>, HttpStore>();

            // register aspnet
            containerRegistry.RegisterSingleton<AspnetService>();
            containerRegistry.RegisterSingleton<IStore<AspnetcoreModel>, AspnetcoreStore>();
        }
    }
}
