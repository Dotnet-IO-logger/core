using Microsoft.Diagnostics.Tracing;
using System;
using System.Collections.Generic;

namespace Diol.Core.TraceEventProcessors
{
    /// <summary>
    /// Represents a class that unsubscribes observers from a trace event router.
    /// </summary>
    public class TraceEventRouterUnSubscriber : IDisposable
    {
        private List<IObserver<TraceEvent>> observers;

        /// <summary>
        /// Initializes a new instance of the <see cref="TraceEventRouterUnSubscriber"/> class.
        /// </summary>
        /// <param name="observers">The list of observers to unsubscribe.</param>
        public TraceEventRouterUnSubscriber(List<IObserver<TraceEvent>> observers)
        {
            this.observers = observers;
        }

        /// <summary>
        /// Disposes the <see cref="TraceEventRouterUnSubscriber"/> instance.
        /// </summary>
        public void Dispose()
        {
            if(this.observers != null)
            {
                this.observers.Clear();
            }
        }
    }
}
