using System;
using System.ServiceModel;
using Rikrop.Core.Wcf.Security.Server;
using Microsoft.Practices.Unity;

namespace Rikrop.Core.Wcf.Unity.ServerRegistration
{
    public class AuthorizationBehaviorRegistrator
    {
        private readonly IUnityContainer _container;

        internal AuthorizationBehaviorRegistrator(IUnityContainer container)
        {
            _container = container;
        }

        public SessionHeaderInfoRegistrator WithStandardAuthorizationManager()
        {
            _container.RegisterType<ServiceAuthorizationManager, AuthorizationManager>(new ContainerControlledLifetimeManager());

            return new SessionHeaderInfoRegistrator(_container);
        }

        public AuthorizationBehaviorRegistrator WithCustomAuthorizationManager<T>(LifetimeManager lifetimeManager = null, params InjectionMember[] injectionMembers)
            where T : ServiceAuthorizationManager
        {
            return WithCustomAuthorizationManager(typeof (T), lifetimeManager, injectionMembers);
        }

        public AuthorizationBehaviorRegistrator WithCustomAuthorizationManager(Type serviceAuthorizationManagerType, LifetimeManager lifetimeManager = null, params InjectionMember[] injectionMembers)
        {
            _container.RegisterType(typeof (ServiceAuthorizationManager), serviceAuthorizationManagerType, lifetimeManager, injectionMembers);

            return this;
        }

        public class Result
        {
            internal Result()
            {

            }
        }
    }
}