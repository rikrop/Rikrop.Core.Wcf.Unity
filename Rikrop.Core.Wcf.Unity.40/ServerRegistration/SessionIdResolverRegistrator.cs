using System;
using Rikrop.Core.Wcf.Security;
using Rikrop.Core.Wcf.Security.Server;
using Microsoft.Practices.Unity;

namespace Rikrop.Core.Wcf.Unity.ServerRegistration
{
    public class SessionIdResolverRegistrator<TSession>
        where TSession : ISession
    {
        private readonly IUnityContainer _container;

        internal SessionIdResolverRegistrator(IUnityContainer container)
        {
            _container = container;
        }

        public SessionRepositoryRegistrator<TSession> WithOperationContextSessionIdResolver()
        {
            return WithCustomSessionIdResolver(typeof (OperationContextSessionIdHolder), new ContainerControlledLifetimeManager());
        }

        public SessionRepositoryRegistrator<TSession> WithCustomSessionIdResolver<T>(LifetimeManager lifetimeManager = null, params InjectionMember[] injectionMembers)
            where T : ISessionIdResolver
        {
            return WithCustomSessionIdResolver(typeof (T), lifetimeManager, injectionMembers);
        }

        public SessionRepositoryRegistrator<TSession> WithCustomSessionIdResolver(Type iSessionIdResolverType, LifetimeManager lifetimeManager = null, params InjectionMember[] injectionMembers)
        {
            _container.RegisterType(typeof (ISessionIdResolver), iSessionIdResolverType, lifetimeManager, injectionMembers);

            return new SessionRepositoryRegistrator<TSession>(_container);
        }
    }
}