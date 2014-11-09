using System;
using System.Reflection;
using Rikrop.Core.Wcf.Security;
using Rikrop.Core.Wcf.Security.Server;
using Microsoft.Practices.Unity;

namespace Rikrop.Core.Wcf.Unity.ServerRegistration
{
    public class AuthStrategyRegistrator
    {
        private readonly IUnityContainer _container;

        internal AuthStrategyRegistrator(IUnityContainer container)
        {
            _container = container;
        }

        public LoginMethodRegistrator<TSession> WithSessionAuthStrategy<TSession>()
            where TSession : ISession
        {
            return WithExtendedSessionAuthStrategy<SessionAuthStrategy<TSession>, TSession>();
        }

        public LoginMethodRegistrator<TSession> WithExtendedSessionAuthStrategy<TAuthStrategy, TSession>()
            where TAuthStrategy : SessionAuthStrategy<TSession>
            where TSession : ISession
        {
            var loginMethodName = Guid.NewGuid().ToString();

            WithCustomAuthStrategy(typeof(TAuthStrategy), new ContainerControlledLifetimeManager(),
                                   new InjectionConstructor(new ResolvedParameter<MethodInfo>(loginMethodName),
                                                            new ResolvedParameter<ISessionRepository<TSession>>(),
                                                            new ResolvedParameter<ISessionIdResolver>()));

            return new LoginMethodRegistrator<TSession>(_container, loginMethodName);
        }

        public AuthorizationBehaviorRegistrator.Result WithCustomAuthStrategy<TAuthStrategy>(LifetimeManager lifetimeManager = null, params InjectionMember[] injectionMembers)
            where TAuthStrategy : IAuthStrategy
        {
            return WithCustomAuthStrategy(typeof (TAuthStrategy), lifetimeManager, injectionMembers);
        }

        public AuthorizationBehaviorRegistrator.Result WithCustomAuthStrategy(Type iAuthStrategyType, LifetimeManager lifetimeManager = null, params InjectionMember[] injectionMembers)
        {
            _container.RegisterType(typeof (IAuthStrategy), iAuthStrategyType, lifetimeManager, injectionMembers);

            return new AuthorizationBehaviorRegistrator.Result();
        }
    }
}