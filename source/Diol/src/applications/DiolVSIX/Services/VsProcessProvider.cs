using Diol.Share.Services;
using EnvDTE80;

namespace DiolVSIX.Services
{
    public class VsProcessProvider : IProcessProvider
    {
        private readonly DTE2 dte;

        public VsProcessProvider(DTE2 dte)
        {
            this.dte = dte;
        }

        public int? GetProcessId()
        {
            int? result = null;

            if (dte.Debugger?.DebuggedProcesses?.Count > 0) 
            {
                foreach (EnvDTE.Process process in this.dte.Debugger.DebuggedProcesses) 
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
