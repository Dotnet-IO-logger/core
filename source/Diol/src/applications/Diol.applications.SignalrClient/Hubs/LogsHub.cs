using Microsoft.AspNetCore.SignalR;

namespace Diol.applications.SignalrClient.Hubs
{
    public class LogsHub : Hub
    {
        public async Task GetProcesses(string message)
        {
            // return all dotnet processes
        }

        public async Task Subscribe(int processId)
        {
            // subscribe
        }

        public async Task Unsubscribe(int processId)
        {
            // unsubscribe
        }
    }
}
