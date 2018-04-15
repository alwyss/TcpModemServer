using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Reflection;

namespace Framework.Utilities.IoC
{
    public class MefContainer : IIocContainer
    {
        private readonly CompositionContainer _container;

        public MefContainer() : this(null)
        {
        }

        public MefContainer(IEnumerable<Assembly> assemblies)
        {
            assemblies = assemblies ?? new Assembly[0];
            var discoveredAssemblies =
                GetAssemblies(new[] { AppDomain.CurrentDomain.BaseDirectory })
                    .Concat(new[] { Assembly.GetEntryAssembly(), Assembly.GetExecutingAssembly() })
                    .Concat(assemblies)
                    .Where(p => p != null).Distinct(new AssemblyEqualityComparer()).ToList();
            _container = new CompositionContainer(new AggregateCatalog(discoveredAssemblies.Select(p => new AssemblyCatalog(p))));
        }

        private IEnumerable<Assembly> GetAssemblies(IEnumerable<string> modulesDirectories)
        {
            var assemblyFiles = new List<string>();
            foreach (var modulesDirectory in modulesDirectories)
            {
                using (var directoryCatalog = new DirectoryCatalog(modulesDirectory))
                    assemblyFiles.AddRange(directoryCatalog.LoadedFiles);
            }

            return assemblyFiles
              .Distinct()
              .Select(Assembly.LoadFrom).ToList();
        }

        public void ComposeInstance<T>(T instance)
        {
            _container.ComposeExportedValue(instance);
        }

        public T GetInstance<T>()
        {
            return _container.GetExportedValue<T>();
        }

        public void ComposeParts(params object[] attributedParts)
        {
            _container.ComposeParts(attributedParts);
        }

        public IEnumerable<T> GetInstances<T>()
        {
            return _container.GetExportedValues<T>();
        }

        public void Dispose()
        {
            var container = _container;
            if (container != null)
            {
                container.Dispose();
            }
        }

        private class AssemblyEqualityComparer : IEqualityComparer<Assembly>
        {
            public bool Equals(Assembly x, Assembly y)
            {
                return x.FullName == y.FullName;
            }

            public int GetHashCode(Assembly obj)
            {
                return obj.GetHashCode();
            }
        }
    }
}