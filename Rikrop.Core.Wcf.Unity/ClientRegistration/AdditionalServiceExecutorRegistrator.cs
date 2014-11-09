using System;
using Rikrop.Core.Framework.Services;
using Microsoft.Practices.Unity;

namespace Rikrop.Core.Wcf.Unity.ClientRegistration
{
    public class AdditionalServiceExecutorRegistrator
    {
        private readonly IUnityContainer _container;
        private readonly Type _serviceExecutorType;
        private readonly LifetimeManager _lifetimeManager;
        private readonly InjectionMember[] _injectionMembers;

        internal AdditionalServiceExecutorRegistrator(IUnityContainer container, Type serviceExecutorType, LifetimeManager lifetimeManager, params InjectionMember[] injectionMembers)
        {
            _container = container;
            _serviceExecutorType = serviceExecutorType;
            _lifetimeManager = lifetimeManager;
            _injectionMembers = injectionMembers;
        }

        public FaultExceptionConverterRegistrator WithExceptionConverters()
        {
            var serviceExecutorName = Guid.NewGuid().ToString();
            Register(serviceExecutorName);

            _container.RegisterType(typeof(IServiceExecutor<>), typeof(ServiceExecutorWithExceptionConversion<>),
                                    new ContainerControlledLifetimeManager(),
                                    new InjectionConstructor(new ResolvedParameter(typeof(IServiceExecutor<>), serviceExecutorName),
                                                             new ResolvedParameter<IFaultExceptionConverter>()));

            return new FaultExceptionConverterRegistrator(_container);
        }

        internal void Register(string serviceExecutorName = null)
        {
            _container.RegisterType(typeof(IServiceExecutor<>), _serviceExecutorType, serviceExecutorName, _lifetimeManager, _injectionMembers);
        }
    }
}