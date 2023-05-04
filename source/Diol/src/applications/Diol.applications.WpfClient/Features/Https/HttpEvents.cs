using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diol.applications.WpfClient.Features.Https
{
    public class HttpRequestStartedEvent : PubSubEvent<string>
    {
    }

    public class HttpRequestEndedEvent : PubSubEvent<string> 
    {
    }

    public class HttpItemSelectedEvent : PubSubEvent<string> 
    {
    }
}
