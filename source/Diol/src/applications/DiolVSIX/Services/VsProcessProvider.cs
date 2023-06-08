using Diol.Share.Services;

namespace DiolVSIX.Services
{
    public class VsProcessProvider : IProcessProvider
    {
        private readonly RequiredServices requiredServices;

        public VsProcessProvider(RequiredServices requiredServices)
        {
            this.requiredServices = requiredServices;
        }

        public int? GetProcessId()
        {
            var dte = this.requiredServices.Dte;

            int? result = null;

            if (dte.Debugger?.DebuggedProcesses?.Count > 0) 
            {
                foreach (EnvDTE.Process process in dte.Debugger.DebuggedProcesses) 
                {
                    result = process.ProcessID;
                    break;
                }

                return result;
            }

            return result;
        }
    }
}
