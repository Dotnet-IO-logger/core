using Diol.Share.Services;
using Diol.Wpf.Core.Features.Shared;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Prism.Events;
using System;

namespace DiolVSIX.Services
{
    public class VsApplicationStateService : IApplicationStateService, IDisposable
    {
        private readonly DebuggerEvents debuggerEvents;
        private readonly IEventAggregator eventAggregator;

        public VsApplicationStateService(
            DebuggerEvents debuggerEvents,
            IEventAggregator eventAggregator)
        {
            this.debuggerEvents = debuggerEvents;
            this.eventAggregator = eventAggregator;
        }

        public void Subscribe()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            this.debuggerEvents.OnEnterRunMode += DebuggerEvents_OnEnterRunMode1;
        }

        private void DebuggerEvents_OnEnterRunMode1(dbgEventReason reason)
        {
            if (reason == dbgEventReason.dbgEventReasonLaunchProgram) 
            {
                this.eventAggregator.GetEvent<DebugModeRunnedEvent>().Publish(true);
            }
        }

        public void Dispose()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            this.debuggerEvents.OnEnterRunMode -= DebuggerEvents_OnEnterRunMode1;
        }
    }
}
