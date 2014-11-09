using System;
using Microsoft.Practices.Unity;

namespace Rikrop.Core.Wcf.Unity.ServerRegistration
{
    public class ServiceHostFactoryRegistrator
    {
        private readonly IUnityContainer _container;

        internal ServiceHostFactoryRegistrator(IUnityContainer container)
        {
            _container = container;
        }

        public RegistrationResult Standard()
        {
            return Custom(typeof (StandardServiceHostFactory), new ContainerControlledLifetimeManager());
        }

        public BehaviorBasedServiceHostFactoryRegistrator WithBehaviors()
        {
            return new BehaviorBasedServiceHostFactoryRegistrator(_container);
        }

        public RegistrationResult Custom<TServiceHostFactory>(LifetimeManager lifetimeManager = null, params InjectionMember[] injectionMembers)
            where TServiceHostFactory : IServiceHostFactory
        {
            return Custom(typeof (TServiceHostFactory), lifetimeManager, injectionMembers);
        }

        public RegistrationResult Custom(Type iServiceHostFactoryType, LifetimeManager lifetimeManager = null, params InjectionMember[] injectionMembers)
        {
            _container.RegisterType(typeof (IServiceHostFactory), iServiceHostFactoryType, lifetimeManager, injectionMembers);

            return new RegistrationResult();
        }
    }
}