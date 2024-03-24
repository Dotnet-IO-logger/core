using Diol.Share.Features.Httpclients;

namespace Diol.Wpf.Core.Features.Https
{
    public class HttpModel 
    {
        public string Key { get; set; }

        public RequestPipelineStartDto Request { get; set; }

        public RequestPipelineRequestHeaderDto RequestMetadata { get; set; }

        public RequestPipelineEndDto Response { get; set; }

        public RequestPipelineResponseHeaderDto ResponseMetadata { get; set; }
    }
}
