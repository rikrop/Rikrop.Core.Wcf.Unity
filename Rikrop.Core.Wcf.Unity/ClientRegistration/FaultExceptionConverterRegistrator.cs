using System;
using Microsoft.Practices.Unity;

namespace Rikrop.Core.Wcf.Unity.ClientRegistration
{
    public class FaultExceptionConverterRegistrator
    {
        private readonly IUnityContainer _container;

        internal FaultExceptionConverterRegistrator(IUnityContainer container)
        {
            _container = container;
        }

        public Result AddFaultToBusinessConverter()
        {
            _container.RegisterType<IFaultExceptionConverter, FaultToBusinessExceptionConverter>(new ContainerControlledLifetimeManager());

            return new Result();
        }

        public Result AddCustomConverter<TConverter>(LifetimeManager lifetimeManager = null, params InjectionMember[] injectionMembers)
            where TConverter : IFaultExceptionConverter
        {
            return AddCustomConverter(typeof(TConverter), lifetimeManager, injectionMembers);
        }

        public Result AddCustomConverter(Type converterType, LifetimeManager lifetimeManager = null, params InjectionMember[] injectionMembers)
        {
            _container.RegisterType(typeof(IFaultExceptionConverter), converterType, lifetimeManager, injectionMembers);

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