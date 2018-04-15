using System;
using System.ComponentModel.Composition;

namespace Framework.Utilities.Helpers
{
    public interface IEventBus
    {
        void Add(object key, Delegate del);
        void Remove(object key, Delegate del);
        void Raise(object key, params object[] args);
    }

    [Export(typeof(IEventBus))]
    public class EventBus : IEventBus
    {
        private readonly EventSet _eventSet = new EventSet();

        public void Add(object key, Delegate del)
        {
            _eventSet.Add(key, del);
        }

        public void Remove(object key, Delegate del)
        {
            _eventSet.Remove(key, del);
        }

        public void Raise(object key, params object[] args)
        {
            _eventSet.Raise(key, args);
        }
    }
}
