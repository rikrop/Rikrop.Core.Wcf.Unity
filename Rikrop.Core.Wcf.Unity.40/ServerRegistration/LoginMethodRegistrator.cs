using System;
using System.Linq.Expressions;
using System.Reflection;
using Rikrop.Core.Wcf.Security;
using Rikrop.Core.Wcf.Security.Server;
using Microsoft.Practices.Unity;

namespace Rikrop.Core.Wcf.Unity.ServerRegistration
{
    public class LoginMethodRegistrator<TSession>
        where TSession : ISession
    {
        private readonly IUnityContainer _container;
        private readonly string _loginMethodName;

        internal LoginMethodRegistrator(IUnityContainer container, string loginMethodName)
        {
            _container = container;
            _loginMethodName = loginMethodName;
        }

        public SessionIdResolverRegistrator<TSession> WithLoginMethod<TContract>(Expression<Action<TContract>> loginMethod)
        {
            return WithLoginMethod(MethodInfoHelper.GetMethodInfo(loginMethod));
        }

        public SessionIdResolverRegistrator<TSession> WithLoginMethod(MethodInfo loginMethod)
        {
            _container.RegisterInstance(_loginMethodName, loginMethod);

            return new SessionIdResolverRegistrator<TSession>(_container);
        }
    }
}