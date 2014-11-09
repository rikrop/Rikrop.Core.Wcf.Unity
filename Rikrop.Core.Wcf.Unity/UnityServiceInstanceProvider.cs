using System;
using System.Diagnostics.Contracts;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using Microsoft.Practices.Unity;

namespace Rikrop.Core.Wcf.Unity
{
    public class UnityServiceInstanceProvider : IInstanceProvider
    {
        private readonly IUnityContainer _container;

        public UnityServiceInstanceProvider(IUnityContainer container)
        {
            Contract.Requires<ArgumentNullException>(container != null);

            _container = container;
        }

        public object GetInstance(InstanceContext instanceContext)
        {
            return _container.Resolve(instanceContext.Host.Description.ServiceType);
        }

        public object GetInstance(InstanceContext instanceContext, Message message)
        {
            return _container.Resolve(instanceContext.Host.Description.ServiceType);
        }

        public void ReleaseInstance(InstanceContext instanceContext, object instance)
        {
            _container.Teardown(instance);
        }
    }
}