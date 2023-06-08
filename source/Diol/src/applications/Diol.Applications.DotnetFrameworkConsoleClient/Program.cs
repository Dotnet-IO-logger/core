using Diol.Core.DiagnosticClients;
using Diol.Share.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Diol.Applications.DotnetFrameworkConsoleClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var builder = EventPipeEventSourceBuilder.CreateDefault()
                .SetConsumers(new List<Diol.Core.Consumers.IConsumer>()
                {
                    new ConsoleConsumer()
                });

            var dotnetProcessesService = new DotnetProcessesService();

            // we expect that the process is running (Diol.applications.PlaygroundApi)
            var processName = "Diol.applications.PlaygroundApi";

            var process = dotnetProcessesService.GetItemOrDefault(processName);

            if (process == null)
            {
                Console.WriteLine($"Process {processName} not found. Please try again");
                return;
            }

            var eventPipeEventSourceWrapper = builder
                .SetProcessId(process.Id)
                .Build();

            // run eventPipeEventSourceWrapper.Start() in a separate thread and wait for a key press to stop it
            var startTask = Task.Run(() =>
            {
                eventPipeEventSourceWrapper.Start();
            });

            var stopTask = Task.Run(() =>
            {
                Console.ReadKey();
                eventPipeEventSourceWrapper.Stop();
            });

            Task.WaitAny(startTask, stopTask);
        }
    }
}
