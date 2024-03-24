using Diol.Core.DiagnosticClients;
using Diol.Core.Features;
using Diol.Core.TraceEventProcessors;
using Diol.Share.Consumers;
using Diol.Share.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Diol.Core
{
    public static class ServiceCollectionExtensions
    {
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
            services.AddSingleton<IProcessor, HttpclientProcessor>();
            services.AddSingleton<IProcessor, EntityFrameworkProcessor>();

            services.AddSingleton<IConsumer, TConsumer>();
            services.AddSingleton<IProcessProvider, TProcessProvider>();
            services.AddSingleton<IApplicationStateService, TApplicationStateService>();

            return services;
        }
    }
}
