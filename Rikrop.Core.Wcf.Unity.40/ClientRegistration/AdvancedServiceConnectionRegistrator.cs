using System;
using Microsoft.Practices.Unity;

namespace Rikrop.Core.Wcf.Unity.ClientRegistration
{
    public class AdvancedServiceConnectionRegistrator
    {
        private readonly IUnityContainer _container;
        private readonly Type _serviceConnectionType;
        private readonly LifetimeManager _lifetimeManager;
        private readonly InjectionMember[] _injectionMembers;

        internal AdvancedServiceConnectionRegistrator(IUnityContainer container, Type serviceConnectionType, LifetimeManager lifetimeManager, params InjectionMember[] injectionMembers)
        {
            _container = container;
            _serviceConnectionType = serviceConnectionType;
            _lifetimeManager = lifetimeManager;
            _injectionMembers = injectionMembers;
        }

        public ServiceConnectionWithBehaviorsRegistrator WithBehaviors()
        {
            string serviceConnectionName = Guid.NewGuid().ToString();

            Register(serviceConnectionName);

            return new ServiceConnectionWithBehaviorsRegistrator(_container, serviceConnectionName);
        }

        internal void Register(string name = null)
        {
            _container.RegisterType(typeof(IServiceConnection), _serviceConnectionType, name, _lifetimeManager, _injectionMembers);
        }
    }
}