using Diol.Share.Features.Aspnetcores;

namespace Diol.Wpf.Core.Features.Aspnetcores
{
    /// <summary>
    /// Represents an ASP.NET Core model.
    /// </summary>
    public class AspnetcoreModel
    {
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the request log.
        /// </summary>
        public RequestLogDto Request { get; set; }

        /// <summary>
        /// Gets or sets the request body metadata.
        /// </summary>
        public RequestBodyDto RequestMetadata { get; set; }

        /// <summary>
        /// Gets or sets the response log.
        /// </summary>
        public ResponseLogDto Response { get; set; }

        /// <summary>
        /// Gets or sets the response body metadata.
        /// </summary>
        public ResponseBodyDto ResponseMetadata { get; set; }
    }
}
