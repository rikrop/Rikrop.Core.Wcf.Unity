using System;
using Microsoft.Practices.Unity;

namespace Rikrop.Core.Wcf.Unity.ServerRegistration
{
    public class FirstStepRegistrator
    {
        private readonly IUnityContainer _container;

        internal FirstStepRegistrator(IUnityContainer container)
        {
            _container = container;
        }

        public SecondStepRegistrator RegisterServiceConnection(Func<ServiceConnectionRegistrator, ServiceConnectionRegistrator.Result> registrator)
        {
            registrator(new ServiceConnectionRegistrator(_container));
            return new SecondStepRegistrator(_container);
        }
    }
}