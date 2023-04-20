using System.Collections.Generic;

namespace Diol.Share.Features.Httpclients
{
    public class RequestPipelineRequestHeaderDto : BaseDto
    {
        public override string CategoryName => "HttpClient";

        public override string EventName => nameof(RequestPipelineRequestHeaderDto);

        public Dictionary<string, string> Headers { get; set; }
    }
}
