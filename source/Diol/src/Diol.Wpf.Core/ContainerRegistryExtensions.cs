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
using System;

namespace Diol.Wpf.Core
{
    public static class ContainerRegistryExtensions
    {
        /// <summary>
        /// Registers the necessary services and dependencies for DiolWpf.
        /// </summary>
        /// <typeparam name="TConsumer">The type of the consumer.</typeparam>
        /// <typeparam name="TProcessProvider">The type of the process provider.</typeparam>
        /// <typeparam name="TApplicationStateService">The type of the application state service.</typeparam>
        /// <param name="containerRegistry">The container registry.</param>
        /// <returns>The updated container registry.</returns>
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
            containerRegistry.Register<IProcessor, HttpClientProcessor>(nameof(HttpClientProcessor));
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

        /// <summary>
        /// Registers the necessary services and dependencies for DiolWpf with custom process provider and application state service.
        /// </summary>
        /// <typeparam name="TConsumer">The type of the consumer.</typeparam>
        /// <typeparam name="TProcessProvider">The type of the process provider.</typeparam>
        /// <typeparam name="TApplicationStateService">The type of the application state service.</typeparam>
        /// <param name="containerRegistry">The container registry.</param>
        /// <param name="processProviderFactoryDelegate">The factory delegate for creating the process provider.</param>
        /// <param name="applicationStateServiceFactoryDelegate">The factory delegate for creating the application state service.</param>
        /// <returns>The updated container registry.</returns>
        public static IContainerRegistry AddDiolWpf<TConsumer, TProcessProvider, TApplicationStateService>(
            this IContainerRegistry containerRegistry,
            Func<IContainerProvider, TProcessProvider> processProviderFactoryDelegate,
            Func<IContainerProvider, TApplicationStateService> applicationStateServiceFactoryDelegate)
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
            containerRegistry.Register<IProcessor, HttpClientProcessor>(nameof(HttpClientProcessor));
            containerRegistry.Register<IProcessor, EntityFrameworkProcessor>(nameof(EntityFrameworkProcessor));

            containerRegistry.RegisterSingleton<IConsumer, TConsumer>();
            containerRegistry.RegisterSingleton<IProcessProvider>(processProviderFactoryDelegate);
            containerRegistry.RegisterSingleton<IApplicationStateService>(applicationStateServiceFactoryDelegate);

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
