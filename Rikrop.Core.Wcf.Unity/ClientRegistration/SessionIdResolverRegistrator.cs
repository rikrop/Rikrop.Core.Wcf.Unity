using System;
using Rikrop.Core.Wcf.Security;
using Rikrop.Core.Wcf.Security.Client;
using Microsoft.Practices.Unity;

namespace Rikrop.Core.Wcf.Unity.ClientRegistration
{
    public class SessionIdResolverRegistrator
    {
        private readonly IUnityContainer _container;
        private readonly ServiceConnectionWithBehaviorsRegistrator _serviceConnectionWithBehaviorsRegistrator;

        internal SessionIdResolverRegistrator(IUnityContainer container, ServiceConnectionWithBehaviorsRegistrator serviceConnectionWithBehaviorsRegistrator)
        {
            _container = container;
            _serviceConnectionWithBehaviorsRegistrator = serviceConnectionWithBehaviorsRegistrator;
        }

        public SessionHeaderMessageInspectorFactoryRegistrator WithStandardSessionIdResolver()
        {
            return WithCustomSessionIdResolver(typeof(SessionIdHolder), new ContainerControlledLifetimeManager());
        }

        public SessionHeaderMessageInspectorFactoryRegistrator WithCustomSessionIdResolver<T>(LifetimeManager lifetimeManager = null, params InjectionMember[] injectionMembers)
            where T : ISessionIdResolver
        {
            return WithCustomSessionIdResolver(typeof(T), lifetimeManager, injectionMembers);
        }

        public SessionHeaderMessageInspectorFactoryRegistrator WithCustomSessionIdResolver(Type iSessionIdResolverType, LifetimeManager lifetimeManager = null, params InjectionMember[] injectionMembers)
        {
            _container.RegisterType(typeof(ISessionIdResolver), iSessionIdResolverType, lifetimeManager, injectionMembers);

            return new SessionHeaderMessageInspectorFactoryRegistrator(_container, _serviceConnectionWithBehaviorsRegistrator);
        }
    }
}