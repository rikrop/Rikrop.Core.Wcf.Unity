using System;
using Microsoft.Practices.Unity;

namespace Rikrop.Core.Wcf.Unity.ClientRegistration
{
    public class ThirdStepRegistrator
    {
        private readonly IUnityContainer _container;

        internal ThirdStepRegistrator(IUnityContainer container)
        {
            _container = container;
        }

        public RegistrationResult RegisterServiceConnection(Func<ServiceConnectionRegistrator, AdvancedServiceConnectionRegistrator> registrator)
        {
            var result = registrator(new ServiceConnectionRegistrator(_container));
            result.Register();

            return new RegistrationResult();
        }

        public RegistrationResult RegisterServiceConnection(Func<ServiceConnectionRegistrator, ServiceConnectionWithBehaviorsRegistrator> registrator)
        {
            var result = registrator(new ServiceConnectionRegistrator(_container));
            result.Register();

            return new RegistrationResult();
        }
    }
}