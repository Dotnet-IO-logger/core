using System;
using System.Collections.Generic;
using System.Text;

namespace Diol.Share.Features.EntityFrameworks
{
    public class CommandExecutedDto : BaseDto
    {
        public override string CategoryName => "EntityFramework";

        public override string EventName => nameof(CommandExecutedDto);

        public TimeSpan Elapsed { get; set; }
    }
}
