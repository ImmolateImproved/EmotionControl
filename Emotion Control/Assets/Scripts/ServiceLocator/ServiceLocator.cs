using System;
using System.Collections.Generic;
using UnityEngine;

namespace Services
{
    public class ServiceLocator : MonoBehaviour
    {
        private static Dictionary<Type, IService> Services;

        //Script execution order - before default time
        private void Awake()
        {
            Services = new Dictionary<Type, IService>();
        }

        public static void Register<TService>(TService service) where TService : IService, new()
        {
            var serviceType = typeof(TService);

            Services.TryAdd(serviceType, service);
        }

        public static TService Get<TService>() where TService : class, IService
        {
            var serviceType = typeof(TService);

            if (Services.TryGetValue(serviceType, out var service))
            {
                return service as TService;
            }

            return null;
        }

        public static bool IsRegistered(Type t)
        {
            return Services.ContainsKey(t);
        }

        public static bool IsRegistered<TService>()
        {
            return IsRegistered(typeof(TService));
        }
    }
}
