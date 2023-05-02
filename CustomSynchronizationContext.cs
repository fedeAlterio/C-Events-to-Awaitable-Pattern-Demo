using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsToAwaitables
{
    public class CustomSynchronizationContext : SynchronizationContext
    {
        readonly BlockingCollection<(SendOrPostCallback @delegate, object? state)> _delegatesWithState = new();
        public CustomSynchronizationContext() => new Thread(Loop).Start();
        public override void Post(SendOrPostCallback d, object? state) => _delegatesWithState.Add((d, state));
        public override void Send(SendOrPostCallback d, object? state) => d(state);

        void Loop()
        {
            SetSynchronizationContext(this);
            foreach(var (@delegate, state) in _delegatesWithState.GetConsumingEnumerable())
            {
                @delegate(state);
            }
        }
    }
}
