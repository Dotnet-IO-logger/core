using Diol.Share.Features.Httpclients;

namespace Diol.Wpf.Core.Features.Https
{
    /// <summary>
    /// Represents an HTTP model.
    /// </summary>
    public class HttpModel
    {
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the request.
        /// </summary>
        public RequestPipelineStartDto Request { get; set; }

        /// <summary>
        /// Gets or sets the request metadata.
        /// </summary>
        public RequestPipelineRequestHeaderDto RequestMetadata { get; set; }

        /// <summary>
        /// Gets or sets the response.
        /// </summary>
        public RequestPipelineEndDto Response { get; set; }

        /// <summary>
        /// Gets or sets the response metadata.
        /// </summary>
        public RequestPipelineResponseHeaderDto ResponseMetadata { get; set; }
    }
}
