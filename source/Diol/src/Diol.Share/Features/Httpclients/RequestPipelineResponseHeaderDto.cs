using System.Collections.Generic;

namespace Diol.Share.Features.Httpclients
{
    /// <summary>
    /// Represents the response header data for the request pipeline.
    /// </summary>
    public class RequestPipelineResponseHeaderDto : BaseDto
    {
        /// <inheritdoc/>
        public override string CategoryName => "HttpClient";

        /// <inheritdoc/>
        public override string EventName => nameof(RequestPipelineResponseHeaderDto);

        /// <summary>
        /// Gets or sets the dictionary of headers.
        /// </summary>
        public Dictionary<string, string> Headers { get; set; }
    }
}
