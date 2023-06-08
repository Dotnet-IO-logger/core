using Diol.Share.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Diol.Share.Services
{
    public class DotnetProcessesService
    {
        public ICollection<DotnetProcessInfo> GetCollection()
        {
            var processes = Process.GetProcesses();

            return processes.Select(x => new DotnetProcessInfo()
            {
                Id = x.Id,
                Name = x.ProcessName
            }).ToList();
        }

        public ICollection<DotnetProcessInfo> GetCollection(string name)
        {
            var processes = Process.GetProcessesByName(name);
            return processes.Select(x => new DotnetProcessInfo()
            {
                Id = x.Id,
                Name = x.ProcessName
            }).ToList();
        }

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
