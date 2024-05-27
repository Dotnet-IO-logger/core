using System.Collections.Generic;

namespace Diol.Share.Features.Httpclients
{
    /// <summary>
    /// Represents the data transfer object for the request headers in the request pipeline.
    /// </summary>
    public class RequestPipelineRequestHeaderDto : BaseDto
    {
        /// <inheritdoc/>
        public override string CategoryName => "HttpClient";

        /// <inheritdoc/>
        public override string EventName => nameof(RequestPipelineRequestHeaderDto);

        /// <summary>
        /// Gets or sets the dictionary of headers.
        /// </summary>
        public Dictionary<string, string> Headers { get; set; }
    }
}
