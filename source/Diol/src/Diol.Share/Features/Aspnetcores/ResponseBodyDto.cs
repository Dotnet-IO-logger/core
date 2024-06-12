using System.Collections.Generic;

namespace Diol.Share.Features.Aspnetcores
{
    /// <summary>
    /// Represents the response body data transfer object.
    /// </summary>
    public class ResponseBodyDto : BaseDto
    {
        /// <inheritdoc/>
        public override string CategoryName => "AspnetCore";

        /// <inheritdoc/>
        public override string EventName => nameof(ResponseBodyDto);

        /// <summary>
        /// Gets or sets the body as a string.
        /// </summary>
        public string BodyAsString { get; set; }

        /// <summary>
        /// Gets or sets the metadata dictionary.
        /// </summary>
        public Dictionary<string, string> Metadata { get; set; }
    }
}
