using Diol.Core.DiagnosticClients;
using Diol.Core.Features;
using Diol.Core.TraceEventProcessors;
using Diol.Share.Consumers;
using Diol.Share.Services;
using Diol.Wpf.Core.Features.Aspnetcores;
using Diol.Wpf.Core.Features.EntityFrameworks;
using Diol.Wpf.Core.Features.Https;
using Diol.Wpf.Core.Services;
using Prism.Ioc;

namespace Diol.Wpf.Core
{
    public static class ContainerRegistryExtensions
    {
        public static IContainerRegistry AddDiolWpf<TConsumer, TProcessProvider, TApplicationStateService>(
            this IContainerRegistry containerRegistry) 
            where TConsumer : class, IConsumer
            where TProcessProvider : class, IProcessProvider
            where TApplicationStateService : class, IApplicationStateService
        {
            // depends of scenario
            containerRegistry.RegisterSingleton<DiolExecutor>();
            containerRegistry.RegisterSingleton<DiolBuilder>();

            containerRegistry.RegisterSingleton<DotnetProcessesService>();
            containerRegistry.RegisterSingleton<EventPublisher>();
            containerRegistry.RegisterSingleton<IProcessorFactory, ProcessorFactory>();
            containerRegistry.RegisterSingleton<EventPipeEventSourceBuilder>();

            // register processors
            containerRegistry.Register<IProcessor, AspnetcoreProcessor>(nameof(AspnetcoreProcessor));
            containerRegistry.Register<IProcessor, HttpclientProcessor>(nameof(HttpclientProcessor));
            containerRegistry.Register<IProcessor, EntityFrameworkProcessor>(nameof(EntityFrameworkProcessor));

            containerRegistry.RegisterSingleton<IConsumer, TConsumer>();
            containerRegistry.RegisterSingleton<IProcessProvider, TProcessProvider>();
            containerRegistry.RegisterSingleton<IApplicationStateService, TApplicationStateService>();

            containerRegistry.RegisterSingleton(typeof(IStore<>), typeof(LocalStore<>));

            // register http
            containerRegistry.RegisterSingleton<HttpService>();
            //containerRegistry.RegisterSingleton<IStore<HttpModel>, LocalStore<HttpModel>>();

            // register aspnet
            containerRegistry.RegisterSingleton<AspnetService>();
            //containerRegistry.RegisterSingleton<IStore<AspnetcoreModel>, LocalStore<AspnetcoreModel>>();

            // register entity framework
            containerRegistry.RegisterSingleton<EntityFrameworkService>();
            //containerRegistry.RegisterSingleton<IStore<EntityFrameworkModel>, LocalStore<EntityFrameworkModel>>();

            return containerRegistry;
        } 
    }
}
