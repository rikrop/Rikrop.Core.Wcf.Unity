using System;
using System.Net;
using Microsoft.Practices.Unity;

namespace Rikrop.Core.Wcf.Unity.ServerRegistration
{
    public class ServiceConnectionRegistrator
    {
        private readonly IUnityContainer _container;

        internal ServiceConnectionRegistrator(IUnityContainer container)
        {
            _container = container;
        }

        public Result NetTcp(string host, int port)
        {
            return Custom(typeof (NetTcpServiceConnection), new ContainerControlledLifetimeManager(), new InjectionConstructor(new InjectionParameter<DnsEndPoint>(new DnsEndPoint(host, port))));
        }

        public Result NamedPipe()
        {
            return Custom(typeof(NamedPipeServiceConnection), new ContainerControlledLifetimeManager(), new InjectionConstructor());
        }

        public Result NamedPipe(string pipeName)
        {
            return Custom(typeof(NamedPipeServiceConnection), new ContainerControlledLifetimeManager(), new InjectionConstructor(new InjectionParameter<string>(pipeName)));
        }

        public Result Custom<TServiceConnection>(LifetimeManager lifetimeManager = null, params InjectionMember[] injectionMembers)
            where TServiceConnection : IServiceConnection
        {
            return Custom(typeof (TServiceConnection), lifetimeManager, injectionMembers);
        }

        public Result Custom(Type iServiceConnectionType, LifetimeManager lifetimeManager = null, params InjectionMember[] injectionMembers)
        {
            _container.RegisterType(typeof (IServiceConnection), iServiceConnectionType, lifetimeManager, injectionMembers);

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