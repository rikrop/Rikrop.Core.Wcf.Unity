using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Dispatcher;
using Rikrop.Core.Framework.Logging;
using Microsoft.Practices.Unity;

namespace Rikrop.Core.Wcf.Unity.ServerRegistration
{
    public class ErrorHandlersBehaviorRegistrator
    {
        private readonly IUnityContainer _container;
        private readonly Dictionary<Type, string> _errorHandlers = new Dictionary<Type, string>();

        internal ErrorHandlersBehaviorRegistrator(IUnityContainer container)
        {
            _container = container;
        }

        public ErrorHandlersBehaviorRegistrator AddLoggingErrorHandler(ILogger logger)
        {
            string loggerContainerName = Guid.NewGuid().ToString();

            _container.RegisterInstance(loggerContainerName, logger);

            return AddLoggingErrorHandler(loggerContainerName);
        }

        public ErrorHandlersBehaviorRegistrator AddLoggingErrorHandler(string loggerContainerName)
        {
            _container.RegisterType<LoggingErrorHandler>(new InjectionConstructor(new ResolvedParameter<ILogger>(loggerContainerName)));
            
            _errorHandlers.Add(typeof(LoggingErrorHandler), loggerContainerName);

            return this;
        }

        public ErrorHandlersBehaviorRegistrator AddBusinessErrorHandler()
        {
            _errorHandlers.Add(typeof(BusinessErrorHandler), null);

            return this;
        }

        public ErrorHandlersBehaviorRegistrator AddCustomErrorHandler<TErrorHandler>()
            where TErrorHandler : IErrorHandler
        {
            return AddCustomErrorHandler(typeof (TErrorHandler));
        }

        public ErrorHandlersBehaviorRegistrator AddCustomErrorHandler(Type iErrorHandler)
        {
            _errorHandlers.Add(iErrorHandler, null);

            return this;
        }

        internal void Register()
        {
            var resolvedParameters = _errorHandlers.Select(o => (object)new ResolvedParameter(o.Key)).ToArray();

            _container.RegisterType<IErrorHandler, AggregatedErrorHandler>(new ContainerControlledLifetimeManager(),
                                                                           new InjectionConstructor(new ResolvedArrayParameter<IErrorHandler>(resolvedParameters)));
        }
    }
}