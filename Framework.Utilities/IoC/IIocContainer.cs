using System;
using System.Collections.Generic;

namespace Framework.Utilities.IoC
{
    public interface IIocContainer : IDisposable
    {
        void ComposeInstance<T>(T instance);

        T GetInstance<T>();

        void ComposeParts(params object[] attributedParts);

        IEnumerable<T> GetInstances<T>();
    }
}