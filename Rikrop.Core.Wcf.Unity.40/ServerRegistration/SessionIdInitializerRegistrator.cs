using System;
using Rikrop.Core.Wcf.Security;
using Rikrop.Core.Wcf.Security.Server;
using Microsoft.Practices.Unity;

namespace Rikrop.Core.Wcf.Unity.ServerRegistration
{
    public class SessionIdInitializerRegistrator
    {
        private readonly IUnityContainer _container;

        internal SessionIdInitializerRegistrator(IUnityContainer container)
        {
            _container = container;
        }

        public AuthStrategyRegistrator WithOperationContextSessionIdInitializer()
        {
            return WithCustomSessionIdInitializer(typeof (OperationContextSessionIdHolder), new ContainerControlledLifetimeManager());
        }

        public AuthStrategyRegistrator WithCustomSessionIdInitializer<T>(LifetimeManager lifetimeManager = null, params InjectionMember[] injectionMembers)
            where T : ISessionIdInitializer
        {
            return WithCustomSessionIdInitializer(typeof (T), lifetimeManager, injectionMembers);
        }

        public AuthStrategyRegistrator WithCustomSessionIdInitializer(Type iSessionIdInitializerType, LifetimeManager lifetimeManager = null, params InjectionMember[] injectionMembers)
        {
            _container.RegisterType(typeof(ISessionIdInitializer), iSessionIdInitializerType, lifetimeManager, injectionMembers);

            return new AuthStrategyRegistrator(_container);
        }
    }
}