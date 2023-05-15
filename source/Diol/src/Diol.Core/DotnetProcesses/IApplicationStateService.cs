using System;
using System.Collections.Generic;
using System.Text;

namespace Diol.Core.DotnetProcesses
{
    public interface IApplicationStateService
    {
        void Subscribe();
    }

    public class LocalApplicationStateService : IApplicationStateService
    {
        public void Subscribe()
        {
        }
    }
}
