using Diol.Share.Features.EntityFrameworks;

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
