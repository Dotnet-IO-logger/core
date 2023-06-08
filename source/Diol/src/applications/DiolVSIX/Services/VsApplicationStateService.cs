using Diol.Share.Services;
using Diol.Wpf.Core.Features.Shared;
using EnvDTE;
using Prism.Events;
using System;

namespace DiolVSIX.Services
{
    public class VsApplicationStateService : IApplicationStateService, IDisposable
    {
        private readonly RequiredServices requiredServices;
        private readonly IEventAggregator eventAggregator;

        public VsApplicationStateService(
            RequiredServices requiredServices,
            IEventAggregator eventAggregator)
        {
            this.requiredServices = requiredServices;
            this.eventAggregator = eventAggregator;
        }

        public void Subscribe()
        {
            this.requiredServices.Dte.Events.DebuggerEvents.OnEnterRunMode += DebuggerEvents_OnEnterRunMode;
            this.requiredServices.Dte.Events.DTEEvents.ModeChanged += DTEEvents_ModeChanged;
        }

        private void DebuggerEvents_OnEnterRunMode(dbgEventReason Reason)
        {
        }

        private void DTEEvents_ModeChanged(vsIDEMode LastMode)
        {
            // vsIDEMode.vsIDEModeDesign -> we moved to Debug mode
            if (LastMode == vsIDEMode.vsIDEModeDesign)
            {
                this.eventAggregator.GetEvent<DebugModeRunnedEvent>().Publish(true);
            }
            // vsIDEMode.vsIDEModeDebug -> we moved out from Design mode
            else 
            {
                this.eventAggregator.GetEvent<DebugModeRunnedEvent>().Publish(false);
            }
        }

        public void Dispose()
        {
            this.requiredServices.Dte.Events.DebuggerEvents.OnEnterRunMode += DebuggerEvents_OnEnterRunMode;
            this.requiredServices.Dte.Events.DTEEvents.ModeChanged -= DTEEvents_ModeChanged;
        }
    }
}
