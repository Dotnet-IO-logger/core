using System;

namespace Diol.Share.Features.Httpclients
{
    /// <summary>
    /// Represents the data transfer object for the end of a request pipeline.
    /// </summary>
    public class RequestPipelineEndDto : BaseDto
    {
        /// <inheritdoc/>
        public override string CategoryName => "HttpClient";

        /// <inheritdoc/>
        public override string EventName => nameof(RequestPipelineEndDto);

        /// <summary>
        /// Gets or sets the status code of the request.
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Gets or sets the elapsed time in milliseconds for the request.
        /// </summary>
        public TimeSpan ElapsedMilliseconds { get; set; }
    }
}
