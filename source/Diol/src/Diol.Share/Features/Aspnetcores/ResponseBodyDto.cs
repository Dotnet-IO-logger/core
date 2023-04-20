using System.Collections.Generic;

namespace Diol.Share.Features.Aspnetcores
{
    public class ResponseBodyDto : BaseDto
    {
        public override string CategoryName => "AspnetCore";

        public override string EventName => nameof(ResponseBodyDto);

        public string BodyAsString { get; set; }

        public Dictionary<string, string> Metadata { get; set; }
    }
}
