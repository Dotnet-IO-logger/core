using System;
using System.Collections.Generic;
using System.Text;

namespace Diol.Share.Services
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
