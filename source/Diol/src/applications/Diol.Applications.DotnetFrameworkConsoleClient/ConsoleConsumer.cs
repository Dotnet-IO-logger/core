using Diol.Core.Consumers;
using Diol.Share.Features;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Diol.Applications.DotnetFrameworkConsoleClient
{
    internal class ConsoleConsumer : IConsumer
    {
        public void OnCompleted()
        {
            // IObservable has finished.
            Debug.WriteLine($"{nameof(ConsoleConsumer)} | {nameof(OnCompleted)}");
        }

        public void OnError(Exception error)
        {
            // write log
            Debug.WriteLine($"{nameof(ConsoleConsumer)} | {nameof(OnError)}");
        }

        public void OnNext(BaseDto value)
        {
            Console.WriteLine();
            Console.WriteLine(JsonSerializer.Serialize(value, value.GetType()));
        }
    }
}
