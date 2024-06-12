namespace Diol.Share.Features.Httpclients
{
    /// <summary>
    /// Represents the data transfer object for the start of a request pipeline.
    /// </summary>
    public class RequestPipelineStartDto : BaseDto
    {
        /// <inheritdoc/>
        public override string CategoryName => "HttpClient";

        /// <inheritdoc/>
        public override string EventName => nameof(RequestPipelineStartDto);

        /// <summary>
        /// Gets or sets the HTTP method of the request.
        /// </summary>
        public string HttpMethod { get; set; }

        /// <summary>
        /// Gets or sets the URI of the request.
        /// </summary>
        public string Uri { get; set; }
    }
}
