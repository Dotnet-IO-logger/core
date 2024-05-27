using System.Collections.Generic;

namespace Diol.Share.Features.Aspnetcores
{
    /// <summary>
    /// Represents a data transfer object for logging response information.
    /// </summary>
    public class ResponseLogDto : BaseDto
    {
        /// <inheritdoc/>
        public override string CategoryName => "AspnetCore";

        /// <inheritdoc/>
        public override string EventName => nameof(ResponseLogDto);

        /// <summary>
        /// Gets or sets the status code of the response.
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Gets or sets the content type of the response.
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// Gets or sets the metadata associated with the response.
        /// </summary>
        public Dictionary<string, string> Metadata { get; set; }
    }
}
