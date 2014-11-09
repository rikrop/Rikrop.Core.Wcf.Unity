using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using Microsoft.Practices.Unity;

namespace Rikrop.Core.Wcf.Unity.ServerRegistration
{
    public class BehaviorBasedServiceHostFactoryRegistrator
    {
        private readonly IUnityContainer _container;
        private readonly List<Type> _behaviors = new List<Type>();

        internal BehaviorBasedServiceHostFactoryRegistrator(IUnityContainer container)
        {
            _container = container;
        }

        public BehaviorBasedServiceHostFactoryRegistrator AddDependencyInjectionBehavior()
        {
            _container.RegisterType<IInstanceProvider, UnityServiceInstanceProvider>(new ContainerControlledLifetimeManager(), new InjectionConstructor(_container));

            _behaviors.Add(typeof (ApplyInstanceProviderServiceBehavior));

            return this;
        }

        public BehaviorBasedServiceHostFactoryRegistrator AddErrorHandlersBehavior(Func<ErrorHandlersBehaviorRegistrator, ErrorHandlersBehaviorRegistrator> registrator)
        {
            var result = registrator(new ErrorHandlersBehaviorRegistrator(_container));
            result.Register();

            _behaviors.Add(typeof (ApplyErrorHandlerServiceBehavior));

            return this;
        }

        public BehaviorBasedServiceHostFactoryRegistrator AddServiceAuthorizationBehavior(Func<AuthorizationBehaviorRegistrator, AuthorizationBehaviorRegistrator.Result> registrator)
        {
            registrator(new AuthorizationBehaviorRegistrator(_container));

            _container.RegisterType<ServiceAuthorizationBehavior>(new ContainerControlledLifetimeManager(),
                                                                  new InjectionFactory(c => new ServiceAuthorizationBehavior
                                                                                                {
                                                                                                    ServiceAuthorizationManager = c.Resolve<ServiceAuthorizationManager>()
                                                                                                }));
            _behaviors.Add(typeof (ServiceAuthorizationBehavior));

            return this;
        }

        public BehaviorBasedServiceHostFactoryRegistrator AddCustomBehavior(Type iServiceBehavior)
        {
            _behaviors.Add(iServiceBehavior);

            return this;
        }

        public BehaviorBasedServiceHostFactoryRegistrator AddCustomBehavior<TBehavior>()
            where TBehavior : IServiceBehavior
        {
            return AddCustomBehavior(typeof (TBehavior));
        }

        internal void Register()
        {
            var resolvedParameters = _behaviors.Select(o => (object) new ResolvedParameter(o)).ToArray();

            _container.RegisterType<IServiceHostFactory, BehaviorBasedServiceHostFactory>(new ContainerControlledLifetimeManager(),
                                                                                          new InjectionConstructor(new ResolvedArrayParameter<IServiceBehavior>(resolvedParameters)));
        }
    }
}