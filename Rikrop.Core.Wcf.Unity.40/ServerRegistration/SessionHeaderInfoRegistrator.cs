using System;
using Rikrop.Core.Wcf.Security;
using Microsoft.Practices.Unity;

namespace Rikrop.Core.Wcf.Unity.ServerRegistration
{
    public class SessionHeaderInfoRegistrator
    {
        private readonly IUnityContainer _container;

        internal SessionHeaderInfoRegistrator(IUnityContainer container)
        {
            _container = container;
        }

        public SessionIdInitializerRegistrator WithStandardSessionHeaderInfo(string headerNamespace, string headerName)
        {
            return WithCustomSessionHeaderInfo(typeof (SessionHeaderInfo), new ContainerControlledLifetimeManager(), new InjectionConstructor(headerNamespace, headerName));
        }

        public SessionIdInitializerRegistrator WithCustomSessionHeaderInfo<T>(LifetimeManager lifetimeManager = null, params InjectionMember[] injectionMembers)
            where T : ISessionHeaderInfo
        {
            return WithCustomSessionHeaderInfo(typeof (T), lifetimeManager, injectionMembers);
        }

        public SessionIdInitializerRegistrator WithCustomSessionHeaderInfo(Type iSessionHeaderInfoType, LifetimeManager lifetimeManager = null, params InjectionMember[] injectionMembers)
        {
            _container.RegisterType(typeof (ISessionHeaderInfo), iSessionHeaderInfoType, lifetimeManager, injectionMembers);

            return new SessionIdInitializerRegistrator(_container);
        }
    }
}