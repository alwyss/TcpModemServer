using System.Threading;

namespace ProviderBacnet
{
    public class AtomicFlag
    {
        private const int False = 0;

        private const int True = 1;

        private int _state = False;

        public AtomicFlag()
        {
        }

        public AtomicFlag(bool state)
        {
            if (state)
            {
                Set();
            }
        }

        public bool CheckAndSet()
        {
            return Interlocked.CompareExchange(ref _state, True, False) == True;
        }

        public void Set()
        {
            _state = True;
        }

        public void Set(bool value)
        {
            if(value) Set();
            else Reset();
        }

        public void Reset()
        {
            _state = False;
        }

        public bool CheckAndReset()
        {
            return Interlocked.CompareExchange(ref _state, False, True) == True;
        }

        public bool Check()
        {
            return _state == True;
        }

        public override string ToString()
        {
            return Check().ToString();
        }
    }
}