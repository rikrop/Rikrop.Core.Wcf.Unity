using System;
using System.Diagnostics.Contracts;
using Rikrop.Core.Wcf.Security.Server;
using Microsoft.Practices.Unity;

namespace Rikrop.Core.Wcf.Unity.ServerRegistration
{
    public class SessionCopierRegistrator<TSession>
        where TSession : ISession
    {
        private readonly IUnityContainer _container;

        internal SessionCopierRegistrator(IUnityContainer container)
        {
            _container = container;
        }

        public AuthorizationBehaviorRegistrator.Result WithStandardSessionCopier()
        {
            Contract.Assume(typeof(Session).IsAssignableFrom(typeof(TSession)), "Стандартный клонатор сессий работает только с классом " + typeof(Session).FullName);

            return WithCustomSessionCopier(typeof(SessionCopier), new ContainerControlledLifetimeManager());
        }

        public AuthorizationBehaviorRegistrator.Result WithCustomSessionCopier<T>(LifetimeManager lifetimeManager = null, params InjectionMember[] injectionMembers)
            where T : ISessionCopier<TSession>
        {
            return WithCustomSessionCopier(typeof(T), lifetimeManager, injectionMembers);
        }

        public AuthorizationBehaviorRegistrator.Result WithCustomSessionCopier(Type iSessionCopierType, LifetimeManager lifetimeManager = null, params InjectionMember[] injectionMembers)
        {
            _container.RegisterType(typeof(ISessionCopier<TSession>), iSessionCopierType, lifetimeManager, injectionMembers);

            return new AuthorizationBehaviorRegistrator.Result();
        }
    }
}