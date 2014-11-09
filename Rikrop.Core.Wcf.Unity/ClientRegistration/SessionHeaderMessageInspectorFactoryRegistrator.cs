using System;
using System.Linq.Expressions;
using System.Reflection;
using Rikrop.Core.Wcf.Security;
using Rikrop.Core.Wcf.Security.Client;
using Microsoft.Practices.Unity;

namespace Rikrop.Core.Wcf.Unity.ClientRegistration
{
    public class SessionHeaderMessageInspectorFactoryRegistrator
    {
        private readonly IUnityContainer _container;
        private readonly ServiceConnectionWithBehaviorsRegistrator _serviceConnectionWithBehaviorsRegistrator;

        internal SessionHeaderMessageInspectorFactoryRegistrator(IUnityContainer container, ServiceConnectionWithBehaviorsRegistrator serviceConnectionWithBehaviorsRegistrator)
        {
            _container = container;
            _serviceConnectionWithBehaviorsRegistrator = serviceConnectionWithBehaviorsRegistrator;
        }

        public ServiceConnectionWithBehaviorsRegistrator WithStandardMessageInspectorFactory<TContract>(Expression<Action<TContract>> loginMethod)
        {
            return WithStandardMessageInspectorFactory(MethodInfoHelper.GetMethodInfo(loginMethod));
        }

        public ServiceConnectionWithBehaviorsRegistrator WithStandardMessageInspectorFactory(MethodInfo loginMethod)
        {
            return WithCustomMessageInspectorFactory(typeof (SessionHeaderMessageInspectorFactory), new ContainerControlledLifetimeManager(),
                                                     new InjectionConstructor(loginMethod, new ResolvedParameter<ISessionIdResolver>(), new ResolvedParameter<ISessionHeaderInfo>()));
        }

        public ServiceConnectionWithBehaviorsRegistrator WithCustomMessageInspectorFactory(Type iSessionHeaderMessageInspectorFactoryType, LifetimeManager lifetimeManager = null, params InjectionMember[] injectionMembers)
        {
            _container.RegisterType(typeof(ISessionHeaderMessageInspectorFactory), iSessionHeaderMessageInspectorFactoryType, lifetimeManager, injectionMembers);

            return _serviceConnectionWithBehaviorsRegistrator;
        }
    }
}