using System.Collections.Generic;

namespace Diol.Share.Features.Aspnetcores
{
    public class RequestLogDto : BaseDto
    {
        public override string CategoryName => "AspnetCore";

        public override string EventName => nameof(RequestLogDto);

        public string Protocol { get; set; }

        public string Method { get; set; }

        public string Scheme { get; set; }

        public string Host { get; set; }

        public string Path { get; set; }

        public Dictionary<string, string> Metadata { get; set; }
    }
}
