using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Description;
using Rikrop.Core.Wcf.Security.Client;
using Microsoft.Practices.Unity;

namespace Rikrop.Core.Wcf.Unity.ClientRegistration
{
    public class ServiceConnectionWithBehaviorsRegistrator
    {
        private readonly IUnityContainer _container;
        private readonly string _serviceConnectionName;

        private readonly List<Type> _behaviors = new List<Type>();

        internal ServiceConnectionWithBehaviorsRegistrator(IUnityContainer container, string serviceConnectionName)
        {
            _container = container;
            _serviceConnectionName = serviceConnectionName;
        }

        public ServiceConnectionWithBehaviorsRegistrator AddSessionBehavior(Func<SessionHeaderInfoRegistrator, ServiceConnectionWithBehaviorsRegistrator> registrator)
        {
            _behaviors.Add(typeof(SessionHeaderMessageInspectorBehavior));

            return registrator(new SessionHeaderInfoRegistrator(_container, this));
        }

        public ServiceConnectionWithBehaviorsRegistrator AddCustomBehavior<TBehavior>()
            where TBehavior : IEndpointBehavior
        {
            return AddCustomBehavior(typeof(TBehavior));
        }

        public ServiceConnectionWithBehaviorsRegistrator AddCustomBehavior(Type behaviorType)
        {
            _behaviors.Add(behaviorType);

            return this;
        }

        internal void Register()
        {
            var resolvedParameters = _behaviors.Select(o => (object)new ResolvedParameter(o)).ToArray();

            _container.RegisterType<IServiceConnection, ServiceConnectionWithBehaviors>(
                new ContainerControlledLifetimeManager(),
                new InjectionConstructor(new ResolvedParameter(typeof(IServiceConnection), _serviceConnectionName),
                                         new ResolvedArrayParameter<IEndpointBehavior>(resolvedParameters)));
        }
    }
}