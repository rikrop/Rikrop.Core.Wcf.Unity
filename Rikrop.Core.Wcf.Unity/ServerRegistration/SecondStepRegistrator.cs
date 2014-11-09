using System;
using Microsoft.Practices.Unity;

namespace Rikrop.Core.Wcf.Unity.ServerRegistration
{
    public class SecondStepRegistrator
    {
        private readonly IUnityContainer _container;

        internal SecondStepRegistrator(IUnityContainer container)
        {
            _container = container;
        }

        public RegistrationResult RegisterServiceHostFactory(Func<ServiceHostFactoryRegistrator, RegistrationResult> registrator)
        {
            return registrator(new ServiceHostFactoryRegistrator(_container));
        }

        public RegistrationResult RegisterServiceHostFactory(Func<ServiceHostFactoryRegistrator, BehaviorBasedServiceHostFactoryRegistrator> registrator)
        {
            var result = registrator(new ServiceHostFactoryRegistrator(_container));
            result.Register();

            return new RegistrationResult();
        }
    }
}