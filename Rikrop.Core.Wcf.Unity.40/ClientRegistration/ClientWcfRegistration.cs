using System;
using Microsoft.Practices.Unity;

namespace Rikrop.Core.Wcf.Unity.ClientRegistration
{
    public static class ClientWcfRegistration
    {
        public static IUnityContainer RegisterClientWcf(this IUnityContainer container, Func<FirstStepRegistrator, RegistrationResult> registrator)
        {
            registrator(new FirstStepRegistrator(container));
            return container;
        }
    }
}