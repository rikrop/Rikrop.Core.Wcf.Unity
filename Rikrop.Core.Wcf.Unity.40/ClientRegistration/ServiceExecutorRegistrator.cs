using System;
using Microsoft.Practices.Unity;

namespace Rikrop.Core.Wcf.Unity.ClientRegistration
{
    public sealed class ServiceExecutorRegistrator
    {
        private readonly IUnityContainer _container;

        internal ServiceExecutorRegistrator(IUnityContainer container)
        {
            _container = container;
        }

        public AdditionalServiceExecutorRegistrator Standard()
        {
            return Custom(typeof(ServiceExecutor<>), new ContainerControlledLifetimeManager());
        }

        public AdditionalServiceExecutorRegistrator WithWrapperCaching()
        {
            return Custom(typeof(ServiceExecutorWithWrapperCaching<>), new ContainerControlledLifetimeManager());
        }

        public AdditionalServiceExecutorRegistrator Custom(Type iServiceExecutorType, LifetimeManager lifetimeManager = null, params InjectionMember[] injectionMembers)
        {
            return new AdditionalServiceExecutorRegistrator(_container, iServiceExecutorType, lifetimeManager, injectionMembers);
        }
    }
}