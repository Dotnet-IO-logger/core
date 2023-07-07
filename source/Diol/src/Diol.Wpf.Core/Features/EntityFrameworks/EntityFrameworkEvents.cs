using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diol.Wpf.Core.Features.EntityFrameworks
{
    public class EntityFrameworkConnectionStartedEvent : PubSubEvent<string>
    {
    }

    public class EntityFrameworkConnectionEndedEvent : PubSubEvent<string>
    {
    }

    public class EntityFrameworkItemSelectedEvent : PubSubEvent<string>
    {
    }
}
