using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Dispatcher;

namespace Framework.Utilities.IoC
{
    public class MefInstanceProvider : IInstanceProvider
    {
        #region Fields

        readonly Type _serviceContractType;

        private static IMefContainer _container;

        #endregion

        #region Constructors

        public static void SetContainer(IMefContainer contianer)
        {
            _container = contianer;
        }

        public MefInstanceProvider(Type t)
        {

            if (t != null && !t.IsInterface)
            {
                throw new ArgumentException("Specified Type must be an interface");
            }

            _serviceContractType = t;
        }

        #endregion

        #region IInstanceProvider Members

        public object GetInstance(InstanceContext instanceContext, System.ServiceModel.Channels.Message message)
        {
            if (_serviceContractType != null)
            {
                ImportDefinition importDefinition =
                    new ImportDefinition(i => i.ContractName.Equals(_serviceContractType.FullName),
                        _serviceContractType.FullName, ImportCardinality.ZeroOrMore, false, false);
                AtomicComposition atomicComposition = new AtomicComposition();
                IEnumerable<Export> extensions;

                bool exportDiscovery = _container.TryGetExports(importDefinition, atomicComposition,
                    out extensions);

                if (exportDiscovery && extensions != null && extensions.Any())
                {
                    return extensions.First().Value;
                }
            }

            return null;
        }

        public object GetInstance(InstanceContext instanceContext)
        {
            return GetInstance(instanceContext, null);
        }

        public void ReleaseInstance(InstanceContext instanceContext, object instance)
        {
            IDisposable disposable = instance as IDisposable;
            if (disposable != null)
            {
                disposable.Dispose();
            }
        }

        #endregion

        #region Private Methods

        #endregion
    }
}
