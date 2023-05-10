using Diol.Share.Features.Aspnetcores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diol.Wpf.Core.Features.Aspnetcores
{
    public class AspnetcoreModel 
    {
        public string Key { get; set; }

        public RequestLogDto? Request { get; set; }

        public RequestBodyDto? RequestMetadata { get; set; }

        public ResponseLogDto? Response { get; set; }

        public ResponseBodyDto? ResponseMetadata { get; set; }
    }
}
