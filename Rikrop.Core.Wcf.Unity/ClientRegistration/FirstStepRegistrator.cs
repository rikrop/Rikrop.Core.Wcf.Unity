using System;
using Microsoft.Practices.Unity;

namespace Rikrop.Core.Wcf.Unity.ClientRegistration
{
    public class FirstStepRegistrator
    {
        private readonly IUnityContainer _container;

        internal FirstStepRegistrator(IUnityContainer container)
        {
            _container = container;
        }

        public SecondStepRegistrator RegisterServiceExecutor(Func<ServiceExecutorRegistrator, AdditionalServiceExecutorRegistrator> reg)
        {
            var result = reg(new ServiceExecutorRegistrator(_container));
            result.Register();

            return new SecondStepRegistrator(_container);
        }

        public SecondStepRegistrator RegisterServiceExecutor(Func<ServiceExecutorRegistrator, FaultExceptionConverterRegistrator.Result> reg)
        {
            reg(new ServiceExecutorRegistrator(_container));

            return new SecondStepRegistrator(_container);
        }
    }
}