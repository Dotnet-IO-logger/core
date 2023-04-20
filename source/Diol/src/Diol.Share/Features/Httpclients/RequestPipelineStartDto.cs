namespace Diol.Share.Features.Httpclients
{
    public class RequestPipelineStartDto : BaseDto
    {
        public override string CategoryName => "HttpClient";

        public override string EventName => nameof(RequestPipelineStartDto);

        public string HttpMethod { get; set; }

        public string Uri { get; set; }
    }
}
