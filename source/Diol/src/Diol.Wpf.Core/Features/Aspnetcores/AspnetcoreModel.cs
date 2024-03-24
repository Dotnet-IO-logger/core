using Diol.Share.Features.Aspnetcores;

namespace Diol.Wpf.Core.Features.Aspnetcores
{
    public class AspnetcoreModel 
    {
        public string Key { get; set; }

        public RequestLogDto Request { get; set; }

        public RequestBodyDto RequestMetadata { get; set; }

        public ResponseLogDto Response { get; set; }

        public ResponseBodyDto ResponseMetadata { get; set; }
    }
}
