using Diol.Share.Features;
using System;

namespace Diol.Share.Consumers
{
    /// <summary>
    /// Represents a consumer that observes and consumes <see cref="BaseDto"/> objects.
    /// </summary>
    public interface IConsumer : IObserver<BaseDto>
    {
    }
}
