using Diol.Aspnet.BackgroundWorkers;
using Diol.Aspnet.Consumers;
using Diol.Aspnet.Hubs;
using Diol.Core;
using Diol.Core.TraceEventProcessors;
using Diol.Share.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Diol.Aspnet
{
    public static class ServiceCollection
    {
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

    public static class  WebApplcationCollection
    {
        public static IEndpointRouteBuilder UseDiol(this IEndpointRouteBuilder builder) 
        {
            builder.MapHub<LogsHub>("/logsHub");

            return builder;
        }
    }
}
