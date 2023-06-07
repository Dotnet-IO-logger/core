using Diol.Core.DotnetProcesses;
using Diol.Wpf.Core.Features.Shared;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Diol.Wpf.Core.Services
{
    public interface IBackendProxy : IDisposable
    {
        Task StartBackend();
    }

    public class DebugBackendProxy : IBackendProxy
    {
        private readonly IEventAggregator eventAggregator;
        private readonly string processName = "diol.applications.signalrclient";
        private readonly DotnetProcessesService dotnetProcessesService;

        public DebugBackendProxy(
            IEventAggregator eventAggregator,
            DotnetProcessesService dotnetProcessesService)
        {
            this.eventAggregator = eventAggregator;
            this.dotnetProcessesService = dotnetProcessesService;
        }

        public async Task StartBackend()
        {
            var services = this.dotnetProcessesService
                .GetItemOrDefault(this.processName);

            var isExist = services != null;

            this.NotifyBackendStatus(isExist);
        }

        public void Dispose()
        {
        }

        private void NotifyBackendStatus(bool isStarted)
        {
            this.eventAggregator
                .GetEvent<BackendStarted>()
                .Publish(isStarted);
        }

    }

    public class ReleaseBackendProxy : IBackendProxy
    {
        private readonly IEventAggregator eventAggregator;
        private int restartCount = 0;
        private ProcessStartInfo processStartInfo;
        private string backendExePath = "backend/diol.applications.signalrclient.exe";
        private Process process;

        public ReleaseBackendProxy(IEventAggregator eventAggregator)
        {
            this.processStartInfo = new ProcessStartInfo()
            {
                CreateNoWindow = false,
                UseShellExecute = false,
                FileName = this.backendExePath,
                WindowStyle = ProcessWindowStyle.Hidden
            };

            this.eventAggregator = eventAggregator;
        }

        public async Task StartBackend()
        {
            try
            {
                var t = Task.Run(() =>
                {
                    this.process = Process.Start(this.processStartInfo);

                    this.NotifyBackendStatus(true);

                    this.process.WaitForExit();
                });
            }
            catch (Exception ex)
            {
                // log
            }
            finally
            {
                this.NotifyBackendStatus(true);
                this.process?.Dispose();
            }
        }

        public void Dispose()
        {
            this.process?.Dispose();
        }

        private void NotifyBackendStatus(bool isStarted)
        {
            this.eventAggregator
                .GetEvent<BackendStarted>()
                .Publish(isStarted);
        }
    }
}
