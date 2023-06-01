using Diol.applications.SignalrClient.BackgroundWorkers;
using Diol.applications.SignalrClient.Consumers;
using Diol.applications.SignalrClient.Hubs;
using Diol.Core.DotnetProcesses;
using Diol.Core.TraceEventProcessors;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR(setting => 
{
    setting.EnableDetailedErrors = true;
});
builder.Services.AddRazorPages();
builder.Services.AddSingleton<EventPublisher>();
builder.Services.AddSingleton<SignalRConsumer>();
builder.Services.AddSingleton<DotnetProcessesService>();
builder.Services.AddHostedService<LogsBackgroundWorker>();
builder.Services.AddSingleton<BackgroundTaskQueue>(ctx =>
{
    return new BackgroundTaskQueue();
});

var app = builder.Build();

app.MapHub<LogsHub>("/logsHub");
app.MapRazorPages();

app.Run();
