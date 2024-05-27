using Diol.Core.DiagnosticClients;
using Diol.Core.Features;
using Diol.Core.TraceEventProcessors;
using Diol.Share.Consumers;
using Diol.Share.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Diol.Core
{
    /// <summary>
    /// Extension methods for configuring Diol Core services.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds Diol Core services to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <typeparam name="TConsumer">The type of the consumer.</typeparam>
        /// <typeparam name="TProcessProvider">The type of the process provider.</typeparam>
        /// <typeparam name="TApplicationStateService">The type of the application state service.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
        /// <returns>The modified <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddDiolCore<
            TConsumer,
            TProcessProvider,
            TApplicationStateService>(
                this IServiceCollection services)
            where TConsumer : class, IConsumer
            where TProcessProvider : class, IProcessProvider
            where TApplicationStateService : class, IApplicationStateService
        {
            services.AddSingleton<DotnetProcessesService>();
            services.AddSingleton<EventPublisher>();
            services.AddSingleton<IProcessorFactory, ProcessorFactory>();
            services.AddSingleton<EventPipeEventSourceBuilder>();

            services.AddSingleton<IProcessor, AspnetcoreProcessor>();
            services.AddSingleton<IProcessor, HttpClientProcessor>();
            services.AddSingleton<IProcessor, EntityFrameworkProcessor>();

            services.AddSingleton<IConsumer, TConsumer>();
            services.AddSingleton<IProcessProvider, TProcessProvider>();
            services.AddSingleton<IApplicationStateService, TApplicationStateService>();

            return services;
        }
    }
}
