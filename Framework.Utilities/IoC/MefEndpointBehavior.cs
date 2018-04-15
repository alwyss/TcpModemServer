using System;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace Framework.Utilities.IoC
{
    public class MefEndpointBehavior : BehaviorExtensionElement, IEndpointBehavior
    {
        #region BehaviorExtensionElement Override

        public override Type BehaviorType
        {
            get { return typeof(MefEndpointBehavior); }
        }

        protected override object CreateBehavior()
        {
            return new MefEndpointBehavior();
        }

        #endregion

        #region IEndpointBehavior Members

        public void AddBindingParameters(ServiceEndpoint endpoint, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            Type contractType = endpoint.Contract.ContractType;
            endpointDispatcher.DispatchRuntime.InstanceProvider = new MefInstanceProvider(contractType);
        }

        public void Validate(ServiceEndpoint endpoint)
        {
        }

        #endregion
    }
}
