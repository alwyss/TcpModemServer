using System;
using System.Collections.Generic;

namespace Framework.Utilities.Helpers
{
    public sealed class EventSet
    {
        private readonly object _lock = new object();
        private readonly Dictionary<object, Delegate> _events = new Dictionary<object, Delegate>();

        public void Add(object eventKey, Delegate handler)
        {
            lock (_lock)
            {
                Delegate d = _events.ValueOrDefault(eventKey);
                _events[eventKey] = Delegate.Combine(d, handler);
            }
        }

        // Removes a delegate from an key (if it exists) and 
        // removes the key -> Delegate mapping the last delegate is removed
        public void Remove(object eventKey, Delegate handler)
        {
            lock (_lock)
            {
                // ensure that an exception is not thrown if
                // attempting to remove a delegate from an key not in the set
                var d = _events.ValueOrDefault(eventKey);
                if (d != null)
                {
                    d = Delegate.Remove(d, handler);

                    // If a delegate remains, set the new head else remove the EventKey
                    if (d != null) _events[eventKey] = d;
                    else _events.Remove(eventKey);
                }
            }
        }

        public Delegate Get(object key)
        {
            lock (_lock)
            {
                return _events.ValueOrDefault(key);
            }
        }

        // Raises the event for the indicated EventKey
        public void Raise(object eventKey, params object[] args)
        {
            // Don't throw an exception if the EventKey is not in the set
            Delegate d;
            lock (_lock)
            {
                d = _events.ValueOrDefault(eventKey);
            }

            if (d != null)
            {
                d.DynamicInvoke(args);
                //d.Method.Invoke(d.Target, new[] { sender, e });
            }
        }

        public void Clear()
        {
            lock (_lock)
            {
                _events.Clear();
            }
        }
    }
}