using Diol.Aspnet.BackgroundWorkers;
using Diol.Aspnet.Consumers;
using Diol.Aspnet.Hubs;
using Diol.Core;
using Diol.Share.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Diol.Aspnet
{
    /// <summary>
    /// Provides extension methods to configure DIOL web services.
    /// </summary>
    public static class ServiceCollection
    {
        /// <summary>
        /// Adds DIOL web services to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
        /// <returns>The modified <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddDiolWeb(this IServiceCollection services)
        {
            services.AddLogging();

            services.AddSignalR(setting =>
            {
                setting.EnableDetailedErrors = true;
            });

            services.AddDiolCore<
                SignalRConsumer,
                LocalDevelopmentProcessProvider,
                LocalApplicationStateService>();

            services.AddHostedService<LogsBackgroundWorker>();
            services.AddSingleton<BackgroundTaskQueue>(ctx =>
            {
                var hubContext = ctx.GetRequiredService<IHubContext<LogsHub>>();
                var loggerFactory = ctx.GetRequiredService<ILoggerFactory>();
                var logger = loggerFactory.CreateLogger<BackgroundTaskQueue>();
                return new BackgroundTaskQueue(hubContext, logger);
            });

            return services;
        }
    }

    /// <summary>
    /// Provides extension methods to configure DIOL web application.
    /// </summary>
    public static class WebApplicationCollection
    {
        /// <summary>
        /// Maps the DIOL logs hub to the specified endpoint.
        /// </summary>
        /// <param name="builder">The <see cref="IEndpointRouteBuilder"/> to map the hub to.</param>
        /// <returns>The modified <see cref="IEndpointRouteBuilder"/>.</returns>
        public static IEndpointRouteBuilder UseDiol(this IEndpointRouteBuilder builder)
        {
            builder.MapHub<LogsHub>("/logsHub");

            return builder;
        }
    }
}
