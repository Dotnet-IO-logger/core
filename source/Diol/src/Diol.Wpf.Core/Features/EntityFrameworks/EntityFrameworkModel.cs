using Diol.Share.Features.EntityFrameworks;

namespace Diol.Wpf.Core.Features.EntityFrameworks
{
    /// <summary>
    /// Represents the Entity Framework model.
    /// </summary>
    public class EntityFrameworkModel
    {
        /// <summary>
        /// Gets or sets the key of the model.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the connection opening DTO.
        /// </summary>
        public ConnectionOpeningDto ConnectionOpening { get; set; }

        /// <summary>
        /// Gets or sets the command executing DTO.
        /// </summary>
        public CommandExecutingDto CommandExecuting { get; set; }

        /// <summary>
        /// Gets or sets the command executed DTO.
        /// </summary>
        public CommandExecutedDto CommandExecuted { get; set; }
    }
}
