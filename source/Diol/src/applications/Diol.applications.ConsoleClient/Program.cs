using Diol.applications.ConsoleClient;
using Diol.Core.DiagnosticClients;
using Diol.Share.Services;
using Microsoft.Extensions.DependencyInjection;

Console.WriteLine("Welcome to Diol Console client");

Console.WriteLine("Registering services");
// register services
var serviceCollection = new ServiceCollection();

serviceCollection.AddSingleton(new DotnetProcessesService());
serviceCollection.AddSingleton<EventPipeEventSourceBuilder>(
    EventPipeEventSourceBuilder.CreateDefault()
    .SetConsumers(new List<Diol.Core.Consumers.IConsumer>() { new ConsoleConsumer() }));

var services = serviceCollection.BuildServiceProvider();

// run app
Console.WriteLine("Running app");

// get process
var dotnetProcessesService = services.GetRequiredService<DotnetProcessesService>();

// we expect that the process is running (Diol.Playgrounds.PlaygroundApi.exe)
var processName = "LogsEfTestApp";

var process = dotnetProcessesService.GetItemOrDefault(processName);

if (process == null) 
{
    Console.WriteLine($"Process {processName} not found. Please try again");
    return;
}

// run process
var builder = services.GetRequiredService<EventPipeEventSourceBuilder>();

var eventPipeEventSourceWrapper = builder
    .SetProcessId(process.Id)
    .Build();

eventPipeEventSourceWrapper.Start();

Console.ReadKey();