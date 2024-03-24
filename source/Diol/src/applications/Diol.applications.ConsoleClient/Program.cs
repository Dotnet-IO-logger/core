using Diol.applications.ConsoleClient;
using Diol.Core;
using Diol.Core.DiagnosticClients;
using Diol.Share.Services;
using Microsoft.Extensions.DependencyInjection;

Console.WriteLine("Welcome to Diol Console client");

// register services
Console.WriteLine("Registering services");
var serviceCollection = new ServiceCollection();
serviceCollection.AddDiolCore<ConsoleConsumer, 
    LocalDevelopmentProcessProvider, 
    LocalApplicationStateService>();

var services = serviceCollection.BuildServiceProvider();

// run app
Console.WriteLine("Running app");

// get process
var dotnetProcessesService = services.GetRequiredService<IProcessProvider>();
var builder = services.GetRequiredService<EventPipeEventSourceBuilder>();

var processId = dotnetProcessesService.GetProcessId();

if (processId == null) 
{
    Console.WriteLine($"Process not found. Please try again");
    return;
}

// run process
var eventPipeEventSourceWrapper = builder
    .SetProcessId(processId.Value)
    .Build();

eventPipeEventSourceWrapper.Start();

Console.ReadKey();