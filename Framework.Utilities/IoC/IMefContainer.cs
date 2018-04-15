using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;

namespace Framework.Utilities.IoC
{
    public interface IMefContainer : IIocContainer
    {
        bool TryGetExports(ImportDefinition importDefinition, AtomicComposition atomicComposition, out IEnumerable<Export> extensions);
    }
}
