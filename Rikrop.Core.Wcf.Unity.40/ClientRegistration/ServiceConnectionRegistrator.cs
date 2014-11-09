using System;
using System.Net;
using Microsoft.Practices.Unity;

namespace Rikrop.Core.Wcf.Unity.ClientRegistration
{
    public class ServiceConnectionRegistrator
    {
        private readonly IUnityContainer _container;

        internal ServiceConnectionRegistrator(IUnityContainer container)
        {
            _container = container;
        }

        public AdvancedServiceConnectionRegistrator NetTcp(string host, int port)
        {
            return Custom(typeof(NetTcpServiceConnection), new ContainerControlledLifetimeManager(), new InjectionConstructor(new InjectionParameter<DnsEndPoint>(new DnsEndPoint(host, port))));
        }

        public AdvancedServiceConnectionRegistrator NamedPipe()
        {
            return Custom(typeof(NamedPipeServiceConnection), new ContainerControlledLifetimeManager(), new InjectionConstructor());
        }

        public AdvancedServiceConnectionRegistrator NamedPipe(string pipeName)
        {
            return Custom(typeof(NamedPipeServiceConnection), new ContainerControlledLifetimeManager(), new InjectionConstructor(new InjectionParameter<string>(pipeName)));
        }

        public AdvancedServiceConnectionRegistrator Custom<TServiceConnection>(LifetimeManager lifetimeManager = null, params InjectionMember[] injectionMembers)
            where TServiceConnection : IServiceConnection
        {
            return Custom(typeof(TServiceConnection), lifetimeManager, injectionMembers);
        }

        public AdvancedServiceConnectionRegistrator Custom(Type serviceConnectionType, LifetimeManager lifetimeManager = null, params InjectionMember[] injectionMembers)
        {
            return new AdvancedServiceConnectionRegistrator(_container, serviceConnectionType, lifetimeManager, injectionMembers);
        }

        public class Result
        {
            internal Result()
            {

            }
        }
    }
}