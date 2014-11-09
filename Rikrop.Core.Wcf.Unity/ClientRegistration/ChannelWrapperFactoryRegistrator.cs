using System;
using Microsoft.Practices.Unity;

namespace Rikrop.Core.Wcf.Unity.ClientRegistration
{
    public class ChannelWrapperFactoryRegistrator
    {
        private readonly IUnityContainer _container;

        internal ChannelWrapperFactoryRegistrator(IUnityContainer container)
        {
            _container = container;
        }

        public Result Standard()
        {
            _container.RegisterType(typeof(IChannelWrapperFactory<>), typeof(StandardChannelWrapperFactory<>), new ContainerControlledLifetimeManager());

            return new Result();
        }

        public Result Custom(Type channelWrapperFactory, LifetimeManager lifetimeManager = null, params InjectionMember[] injectionMembers)
        {
            _container.RegisterType(typeof(IChannelWrapperFactory<>), channelWrapperFactory, lifetimeManager, injectionMembers);

            return new Result();
        }

        public class Result
        {
            internal Result()
            {

            }
        }
    }
}