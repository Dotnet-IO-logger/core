using System;

namespace Diol.Share.Features.Httpclients
{
    public class RequestPipelineEndDto : BaseDto
    {
        public override string CategoryName => "HttpClient";

        public override string EventName => nameof(RequestPipelineEndDto);

        public int StatusCode { get; set; }

        public TimeSpan ElapsedMilliseconds { get; set; }
    }
}
