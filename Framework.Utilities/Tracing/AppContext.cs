using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Framework.Utilities.IoC;
using ProviderBacnet;

namespace Framework.Utilities.Tracing
{
    public class AppContext : IDisposable
    {
        private readonly AtomicFlag _isDisposed = new AtomicFlag();

        public AppContext(string appName, TracingOutput tracingOutput = TracingOutput.File, TracerType tracerType = TracerType.Default, bool debugMode = false, IEnumerable<Assembly> assemblies = null)
        {
            Container = new MefContainer(assemblies);
            Container.ComposeInstance(Container);

            var tracer = Container.GetInstances<ITracer>().First(t => t.TracerType == tracerType);
            tracer.Initialize(appName, tracingOutput, debugMode);
            TraceUtil.SetTracer(tracer);
            Tracer = tracer;
        }

        public TService GetService<TService>()
        {
            return Container.GetInstance<TService>();
        }

        public void Cleanup()
        {
            Tracer.Dispose();
            Container.Dispose();
            TraceUtil.SetTracer(null);
        }

        public IIocContainer Container { get; private set; }

        public ITracer Tracer { get; private set; }

        public void Dispose()
        {
            if (!_isDisposed.CheckAndSet())
            {
                Cleanup();
            }
        }
    }
}
