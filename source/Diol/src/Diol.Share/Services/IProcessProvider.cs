using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Diol.Share.Services
{
    public interface IProcessProvider
    {
        int? GetProcessId();
    }

    public class LocalDevelopmentProcessProvider : IProcessProvider
    {
        private readonly DotnetProcessesService dotnetService;

        public LocalDevelopmentProcessProvider(DotnetProcessesService dotnetService)
        {
            this.dotnetService = dotnetService;
        }

        public int? GetProcessId()
        {
            // we expect that the process is running (Diol.Playgrounds.PlaygroundApi.exe)
            var processName = "Diol.Playgrounds.PlaygroundApi";
            var process = this.dotnetService.GetItemOrDefault(processName);

            if (process == null)
            {
                Debug.WriteLine($"Process {processName} not found. Please try again");
            }

            return process?.Id;
        }
    }
}
