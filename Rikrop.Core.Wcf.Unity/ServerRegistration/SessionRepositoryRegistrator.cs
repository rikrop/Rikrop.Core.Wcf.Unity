using System;
using Rikrop.Core.Wcf.Security.Server;
using Microsoft.Practices.Unity;

namespace Rikrop.Core.Wcf.Unity.ServerRegistration
{
    public class SessionRepositoryRegistrator<TSession>
        where TSession : ISession
    {
        private readonly IUnityContainer _container;

        internal SessionRepositoryRegistrator(IUnityContainer container)
        {
            _container = container;
        }

        public SessionCopierRegistrator<TSession> WithInMemorySessionRepository()
        {
            WithCustomSessionRepository(typeof (InMemorySessionRepository<TSession>), new ContainerControlledLifetimeManager());

            return new SessionCopierRegistrator<TSession>(_container);
        }

        public AuthorizationBehaviorRegistrator.Result WithCustomSessionRepository<T>(LifetimeManager lifetimeManager = null, params InjectionMember[] injectionMembers)
            where T : ISessionRepository<TSession>
        {
            return WithCustomSessionRepository(typeof (T), lifetimeManager, injectionMembers);
        }

        public AuthorizationBehaviorRegistrator.Result WithCustomSessionRepository(Type iSessionRepositoryType, LifetimeManager lifetimeManager = null, params InjectionMember[] injectionMembers)
        {
            _container.RegisterType(typeof (ISessionRepository<TSession>), iSessionRepositoryType, lifetimeManager, injectionMembers);

            return new AuthorizationBehaviorRegistrator.Result();
        }
    }
}