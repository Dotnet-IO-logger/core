using System.Collections.Generic;

namespace Diol.Share.Features.Aspnetcores
{
    /// <summary>
    /// Represents a data transfer object for logging request information.
    /// </summary>
    public class RequestLogDto : BaseDto
    {
        /// <inheritdoc/>
        public override string CategoryName => "AspnetCore";

        /// <inheritdoc/>
        public override string EventName => nameof(RequestLogDto);

        /// <summary>
        /// Gets or sets the protocol of the request.
        /// </summary>
        public string Protocol { get; set; }

        /// <summary>
        /// Gets or sets the HTTP method of the request.
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// Gets or sets the scheme of the request.
        /// </summary>
        public string Scheme { get; set; }

        /// <summary>
        /// Gets or sets the host of the request.
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Gets or sets the path of the request.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets the metadata associated with the request.
        /// </summary>
        public Dictionary<string, string> Metadata { get; set; }
    }
}
