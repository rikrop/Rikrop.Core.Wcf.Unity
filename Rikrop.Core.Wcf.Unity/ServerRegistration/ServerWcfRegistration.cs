using System;
using Microsoft.Practices.Unity;

namespace Rikrop.Core.Wcf.Unity.ServerRegistration
{
    public static class ServerWcfRegistration
    {
        public static IUnityContainer RegisterServerWcf(this IUnityContainer container, Func<FirstStepRegistrator, RegistrationResult> registrator)
        {
            registrator(new FirstStepRegistrator(container));
            return container;
        }
    }
}