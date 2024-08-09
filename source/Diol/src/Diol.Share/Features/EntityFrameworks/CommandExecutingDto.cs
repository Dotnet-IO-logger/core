namespace Diol.Share.Features.EntityFrameworks
{
    /// <summary>
    /// Represents a data transfer object for executing commands in Entity Framework.
    /// </summary>
    public class CommandExecutingDto : BaseDto
    {
        /// <summary>
        /// Gets or sets the parameters for the command.
        /// </summary>
        public string Parameters { get; set; }

        /// <summary>
        /// Gets or sets the text of the command.
        /// </summary>
        public string CommandText { get; set; }

        /// <summary>
        /// Gets or sets the Operation Name.
        /// </summary>
        public string OperationName { get; set; }

        /// <summary>
        /// Gets or sets the Table Name.
        /// </summary>
        public string TableName { get; set; }

        /// <inheritdoc/>
        public override string CategoryName => "EntityFramework";

        /// <inheritdoc/>
        public override string EventName => nameof(CommandExecutingDto);

        /// <summary>
        /// Flag is the query is a part of a transaction.
        /// </summary>
        public bool IsTransaction { get; set; }
    }
}
