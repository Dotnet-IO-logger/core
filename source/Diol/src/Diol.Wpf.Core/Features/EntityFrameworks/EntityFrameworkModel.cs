using Diol.Share.Features.EntityFrameworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diol.Wpf.Core.Features.EntityFrameworks
{
    public class EntityFrameworkModel
    {
        public string Key { get; set; }

        public ConnectionOpeningDto ConnectionOpening { get; set; }

        public CommandExecutingDto CommandExecuting { get; set; }

        public CommandExecutedDto CommandExecuted { get; set; }
    }
}
