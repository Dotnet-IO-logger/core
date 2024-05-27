using Diol.Share.Utils;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Diol.Share.Services
{
    /// <summary>
    /// Provides methods to retrieve information about dotnet processes.
    /// </summary>
    public class DotnetProcessesService
    {
        /// <summary>
        /// Retrieves a collection of all dotnet processes.
        /// </summary>
        /// <returns>A collection of <see cref="DotnetProcessInfo"/> objects representing the dotnet processes.</returns>
        public ICollection<DotnetProcessInfo> GetCollection()
        {
            var processes = Process.GetProcesses();

            return processes.Select(x => new DotnetProcessInfo()
            {
                Id = x.Id,
                Name = x.ProcessName
            }).ToList();
        }

        /// <summary>
        /// Retrieves a collection of dotnet processes with the specified name.
        /// </summary>
        /// <param name="name">The name of the dotnet processes to retrieve.</param>
        /// <returns>A collection of <see cref="DotnetProcessInfo"/> objects representing the dotnet processes with the specified name.</returns>
        public ICollection<DotnetProcessInfo> GetCollection(string name)
        {
            var processes = Process.GetProcessesByName(name);
            return processes.Select(x => new DotnetProcessInfo()
            {
                Id = x.Id,
                Name = x.ProcessName
            }).ToList();
        }

        /// <summary>
        /// Retrieves the first dotnet process with the specified name, or null if no such process exists.
        /// </summary>
        /// <param name="name">The name of the dotnet process to retrieve.</param>
        /// <returns>A <see cref="DotnetProcessInfo"/> object representing the dotnet process with the specified name, or null if no such process exists.</returns>
        public DotnetProcessInfo GetItemOrDefault(string name)
        {
            var process = Process.GetProcessesByName(name)
                .Select(x => new DotnetProcessInfo()
                {
                    Id = x.Id,
                    Name = x.ProcessName
                })
                .FirstOrDefault();

            return process;
        }

        /// <summary>
        /// Retrieves the first dotnet process with the specified name.
        /// </summary>
        /// <param name="name">The name of the dotnet process to retrieve.</param>
        /// <returns>A <see cref="DotnetProcessInfo"/> object representing the dotnet process with the specified name.</returns>
        /// <exception cref="InvalidOperationException">Thrown if no dotnet process with the specified name exists.</exception>
        public DotnetProcessInfo GetItem(string name)
        {
            var process = Process.GetProcessesByName(name)
                .Select(x => new DotnetProcessInfo()
                {
                    Id = x.Id,
                    Name = x.ProcessName
                })
                .First();

            return process;
        }
    }
}
