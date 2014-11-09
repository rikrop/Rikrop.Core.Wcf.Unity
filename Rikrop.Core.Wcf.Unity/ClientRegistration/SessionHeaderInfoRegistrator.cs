using System;
using Rikrop.Core.Wcf.Security;
using Microsoft.Practices.Unity;

namespace Rikrop.Core.Wcf.Unity.ClientRegistration
{
    public class SessionHeaderInfoRegistrator
    {
        private readonly IUnityContainer _container;
        private readonly ServiceConnectionWithBehaviorsRegistrator _serviceConnectionWithBehaviorsRegistrator;

        internal SessionHeaderInfoRegistrator(IUnityContainer container, ServiceConnectionWithBehaviorsRegistrator serviceConnectionWithBehaviorsRegistrator)
        {
            _container = container;
            _serviceConnectionWithBehaviorsRegistrator = serviceConnectionWithBehaviorsRegistrator;
        }

        public SessionIdResolverRegistrator WithStandardSessionHeaderInfo(string headerNamespace, string headerName)
        {
            return WithCustomSessionHeaderInfo(typeof(SessionHeaderInfo), new ContainerControlledLifetimeManager(), new InjectionConstructor(headerNamespace, headerName));
        }

        public SessionIdResolverRegistrator WithCustomSessionHeaderInfo<T>(LifetimeManager lifetimeManager = null, params InjectionMember[] injectionMembers)
            where T : ISessionHeaderInfo
        {
            return WithCustomSessionHeaderInfo(typeof (T), lifetimeManager, injectionMembers);
        }

        public SessionIdResolverRegistrator WithCustomSessionHeaderInfo(Type iSessionHeaderInfoType, LifetimeManager lifetimeManager = null, params InjectionMember[] injectionMembers)
        {
            _container.RegisterType(typeof(ISessionHeaderInfo), iSessionHeaderInfoType, lifetimeManager, injectionMembers);

            return new SessionIdResolverRegistrator(_container, _serviceConnectionWithBehaviorsRegistrator);
        }

    }
}