using System.Collections.Generic;

namespace Diol.Share.Features.Httpclients
{
    public class RequestPipelineResponseHeaderDto : BaseDto
    {
        public override string CategoryName => "HttpClient";

        public override string EventName => nameof(RequestPipelineResponseHeaderDto);

        public Dictionary<string, string> Headers { get; set; }
    }
}
