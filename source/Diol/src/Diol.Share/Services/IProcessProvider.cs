using System.Diagnostics;

namespace Diol.Share.Services
{
    /// <summary>
    /// Represents a provider for retrieving process information.
    /// </summary>
    public interface IProcessProvider
    {
        /// <summary>
        /// Gets the process ID.
        /// </summary>
        /// <returns>The process ID, or null if the process is not found.</returns>
        int? GetProcessId();
    }

    public class LocalDevelopmentProcessProvider : IProcessProvider
    {
        private readonly DotnetProcessesService dotnetService;

        public LocalDevelopmentProcessProvider(DotnetProcessesService dotnetService)
        {
            this.dotnetService = dotnetService;
        }

        /// <inheritdoc/>
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
