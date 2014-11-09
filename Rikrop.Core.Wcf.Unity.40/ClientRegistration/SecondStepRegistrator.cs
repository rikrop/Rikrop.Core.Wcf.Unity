using System;
using Microsoft.Practices.Unity;

namespace Rikrop.Core.Wcf.Unity.ClientRegistration
{
    public class SecondStepRegistrator
    {
        private readonly IUnityContainer _container;

        internal SecondStepRegistrator(IUnityContainer container)
        {
            _container = container;
        }

        public ThirdStepRegistrator RegisterChannelWrapperFactory(Func<ChannelWrapperFactoryRegistrator, ChannelWrapperFactoryRegistrator.Result> registrator)
        {
            registrator(new ChannelWrapperFactoryRegistrator(_container));
            return new ThirdStepRegistrator(_container);
        }
    }
}