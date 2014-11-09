using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using Microsoft.Practices.Unity;

namespace Rikrop.Core.Wcf.Unity
{
    public static class RrcWcfUnityExtensions
    {
        /// <summary>
        ///   Регистрирует:
        ///   IServiceHostFactory -> StandardServiceHostFactory.
        ///   IServiceConnection -> NetTcpServiceConnection.
        /// </summary>
        public static IUnityContainer RegisterWcfHosting(this IUnityContainer container, string serviceIp, int servicePort)
        {
            container.RegisterType<IServiceHostFactory, StandardServiceHostFactory>(new ContainerControlledLifetimeManager());
            container.RegisterInstance<IServiceConnection>(new NetTcpServiceConnection(new DnsEndPoint(serviceIp, servicePort)));

            return container;
        }

        /// <summary>
        ///   Регистрирует:
        ///   IServiceHostFactory -> BehaviorBasedServiceHostFactory.
        ///   IServiceConnection -> NetTcpServiceConnection.
        /// 
        /// Для BehaviorBasedServiceHostFactory регистрирует список передаваемых IServiceBehavior'ов.
        /// </summary>
        public static IUnityContainer RegisterWcfHosting(this IUnityContainer container, string serviceIp, int servicePort, IEnumerable<Type> serviceBehaviors)
        {
            var resolvedParameters = serviceBehaviors.Select(o => (object)new ResolvedParameter(o)).ToArray();

            container.RegisterType<IServiceHostFactory, BehaviorBasedServiceHostFactory>(new ContainerControlledLifetimeManager(),
                                                                                         new InjectionConstructor(new ResolvedArrayParameter<IServiceBehavior>(resolvedParameters)));
            container.RegisterInstance<IServiceConnection>(new NetTcpServiceConnection(new DnsEndPoint(serviceIp, servicePort)));

            return container;
        }

        /// <summary>
        ///   Регистрирует для IErrorHandler'a класс AggregatedErrorHandler.
        /// 
        ///   Принимает список типов, реализующих интерфейс IErrorHandler.
        ///   Проверка исключений будет идти в порядке регистрации обработчиков в списке.
        /// </summary>
        public static IUnityContainer RegisterWcfErrorHandlers(this IUnityContainer container, IEnumerable<Type> errorHandlers)
        {
            var resolvedParameters = errorHandlers.Select(o => (object)new ResolvedParameter(o)).ToArray();

            container.RegisterType<IErrorHandler, AggregatedErrorHandler>(new ContainerControlledLifetimeManager(),
                                                                          new InjectionConstructor(new ResolvedArrayParameter<IErrorHandler>(resolvedParameters)));

            return container;
        }
    }
}