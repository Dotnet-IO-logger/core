namespace Diol.Share.Features
{
    /// <summary>
    /// Base class for Data Transfer Objects (DTOs).
    /// </summary>
    public abstract class BaseDto
    {
        /// <summary>
        /// Gets or sets the correlation ID.
        /// </summary>
        public string CorrelationId { get; set; }

        /// <summary>
        /// Gets the category name.
        /// </summary>
        public abstract string CategoryName { get; }

        /// <summary>
        /// Gets the event name.
        /// </summary>
        public abstract string EventName { get; }

        /// <summary>
        /// Gets or sets the process ID.
        /// </summary>
        public int ProcessId { get; set; }

        /// <summary>
        /// Gets or sets the process name.
        /// </summary>
        public string ProcessName { get; set; }
    }
}
