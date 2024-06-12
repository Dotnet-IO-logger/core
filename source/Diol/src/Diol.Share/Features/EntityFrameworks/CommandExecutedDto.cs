using System;

namespace Diol.Share.Features.EntityFrameworks
{
    /// <summary>
    /// Represents a data transfer object for a command executed event in Entity Framework.
    /// </summary>
    public class CommandExecutedDto : BaseDto
    {
        /// <inheritdoc/>
        public override string CategoryName => "EntityFramework";

        /// <inheritdoc/>
        public override string EventName => nameof(CommandExecutedDto);

        /// <summary>
        /// Gets or sets the elapsed time in milliseconds for the command execution.
        /// </summary>
        public TimeSpan ElapsedMilliseconds { get; set; }
    }
}
