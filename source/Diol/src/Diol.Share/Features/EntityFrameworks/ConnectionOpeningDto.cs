using Diol.Share.Features.Aspnetcores;
using System;
using System.Collections.Generic;
using System.Text;

namespace Diol.Share.Features.EntityFrameworks
{
    public class ConnectionOpeningDto : BaseDto
    {
        public override string CategoryName => "EntityFramework";

        public override string EventName => nameof(ConnectionOpeningDto);

        public string Database { get; set; }

        public string Server { get; set; }
    }
}
