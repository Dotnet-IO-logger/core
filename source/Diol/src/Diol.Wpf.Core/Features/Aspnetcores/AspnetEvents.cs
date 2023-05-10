using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diol.Wpf.Core.Features.Aspnetcores
{
    public class AspnetRequestStartedEvent : PubSubEvent<string>
    {
    }

    public class AspnetRequestEndedEvent : PubSubEvent<string>
    {
    }

    public class AspnetItemSelectedEvent : PubSubEvent<string>
    {
    }
}
