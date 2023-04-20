using Diol.Share.Features;
using System.Diagnostics;
using System.Text.Json;

namespace Diol.Core.Consumers
{
    public class ConsoleConsumer : IConsumer
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
