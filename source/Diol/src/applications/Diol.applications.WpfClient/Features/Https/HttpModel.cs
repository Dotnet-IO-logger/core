using Diol.Share.Features.Httpclients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diol.applications.WpfClient.Features.Https
{
    public class HttpModel 
    {
        public string Key { get; set; }

        public RequestPipelineStartDto? Request { get; set; }

        public RequestPipelineRequestHeaderDto? RequestMetadata { get; set; }

        public RequestPipelineEndDto? Response { get; set; }

        public RequestPipelineResponseHeaderDto? ResponseMetadata { get; set; }
    }
}
