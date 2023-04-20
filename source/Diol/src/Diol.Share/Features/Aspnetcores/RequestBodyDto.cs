using System.Collections.Generic;

namespace Diol.Share.Features.Aspnetcores
{
    public class RequestBodyDto : BaseDto
    {
        public override string CategoryName => "AspnetCore";

        public override string EventName => nameof(RequestBodyDto);

        public string BodyAsString { get; set; }

        public Dictionary<string, string> Metadata { get; set; }
    }
}
