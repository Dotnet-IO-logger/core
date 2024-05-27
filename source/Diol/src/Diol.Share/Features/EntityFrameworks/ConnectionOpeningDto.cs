namespace Diol.Share.Features.EntityFrameworks
{
    /// <summary>
    /// Represents a data transfer object for opening a connection in Entity Framework.
    /// </summary>
    public class ConnectionOpeningDto : BaseDto
    {
        /// <inheritdoc/>
        public override string CategoryName => "EntityFramework";

        /// <inheritdoc/>
        public override string EventName => nameof(ConnectionOpeningDto);

        /// <summary>
        /// Gets or sets the name of the database.
        /// </summary>
        public string Database { get; set; }

        /// <summary>
        /// Gets or sets the name of the server.
        /// </summary>
        public string Server { get; set; }
    }
}
