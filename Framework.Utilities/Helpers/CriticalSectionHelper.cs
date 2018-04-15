using System;
using ProviderBacnet;

namespace Framework.Utilities.Helpers
{
    public class CriticalSectionHelper
    {
        public static RunOnceState RunOnce(AtomicFlag flag)
        {
            return new RunOnceState(flag);
        }

        public class RunOnceState : IDisposable
        {
            private readonly AtomicFlag _flag;

            public RunOnceState(AtomicFlag flag)
            {
                _flag = flag;
                Run = !flag.CheckAndSet();
            }

            public bool Run { get; private set; }

            public void Dispose()
            {
                if(!Run)
                    _flag.Reset();
            }
        }
    }
}
