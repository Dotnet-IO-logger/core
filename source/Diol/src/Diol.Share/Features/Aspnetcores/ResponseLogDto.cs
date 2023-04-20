using System.Collections.Generic;

namespace Diol.Share.Features.Aspnetcores
{
    public class ResponseLogDto : BaseDto
    {
        public override string CategoryName => "AspnetCore";

        public override string EventName => nameof(ResponseLogDto);

        public int StatusCode { get; set; }

        public string? ContentType { get; set; }

        public Dictionary<string, string> Metadata { get; set; }
    }
}
