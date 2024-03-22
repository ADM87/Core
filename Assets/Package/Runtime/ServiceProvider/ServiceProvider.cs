using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using static ADM.Assert;

namespace ADM
{
    public class ServiceProvider
    {
        private static Dictionary<Type, ServiceInfo> k_services = new();

        internal static void CollectServiceDefinitions()
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (type.IsAbstract)
                        continue;

                    var attribute = type.GetCustomAttribute<ServiceDefinitionAttribute>(true);
                    if (attribute == null)
                        continue;

                    AddService(type, attribute);
                }
            }
        }
        
        private static void AddService(Type type, ServiceDefinitionAttribute attribute)
        {
            ASSERT_FALSE(k_services.ContainsKey(attribute.Interface),
                $"Definition for {attribute.Interface.Name} already exists");

            ASSERT_FALSE(type.IsAbstract,
                $"Implementation {type.Name} cannot be an abstract type");

            ASSERT_FALSE(k_services.Values.Any(service => service.Implementation.Equals(type)),
                $"Implementation {type.Name} already exists for another service");

            ASSERT_TRUE(attribute.Interface.IsAssignableFrom(type),
                $"Implementation {type.Name} must derive from {attribute.Interface.Name}");

            k_services.Add(attribute.Interface, new ServiceInfo
            {
                Interface       = attribute.Interface,
                Implementation  = type,
                Dependencies    = GetDependencies(type),
                IsSingleton     = attribute.IsSingleton
            });
        }

        private static Type[] GetDependencies(Type type)
        {
            return GetConstructor(type)
                .GetParameters()
                .Select(info => info.ParameterType)
                .ToArray();
        }

        private static ConstructorInfo GetConstructor(Type type)
        {
            var constructors = type.GetConstructors();

            ASSERT_TRUE(constructors.Length == 1, 
                $"Invalid number of contructors found on service {type.Name}. Expected 1, found {constructors.Length}");

            return constructors[0];
        }

        public static T Get<T>()
        {
            return (T)Get(typeof(T));
        }

        public static object Get(Type type)
        {
            ASSERT_TRUE(type.IsInterface,
                $"{type.Name} is not a service interface. You can only access a service by providing it's interface type");

            ASSERT_TRUE(k_services.ContainsKey(type),
                $"{type.Name} has not been registered. Are you missing a ServiceDefinition attribute somewhere?");

            ServiceInfo serviceInfo = k_services[type];

            if (serviceInfo.Instance != null)
                return serviceInfo.Instance;

            foreach (Type dependency in serviceInfo.Dependencies)
            {
                ASSERT_TRUE(dependency.IsInterface,
                    $"{dependency.Name} is not a service interface. Service dependencies must be service interfaces");

                ASSERT_TRUE(k_services.ContainsKey(dependency),
                    $"Missing service definition for {dependency.Name}");

                ASSERT_FALSE(IsCircular(serviceInfo.Implementation, dependency),
                    $"Detected circular dependency for {serviceInfo.Implementation.Name} and {k_services[dependency].Implementation.Name}");
            }

            return ConstructService(k_services[type]);
        }

        public static IEnumerable<T> GetAll<T>()
        {
            return k_services
                .Where(kvp => typeof(T).IsAssignableFrom(kvp.Key))
                .Select(kvp => (T)Get(kvp.Key));
        }

        private static bool IsCircular(Type type, Type dependency)
        {
            ServiceInfo serviceInfo = k_services[dependency];

            if (type.Equals(serviceInfo.Implementation))
                return true;

            foreach (Type depType in serviceInfo.Dependencies)
            {
                if (IsCircular(type, depType))
                    return true;
            }

            return false;
        }

        private static object ConstructService(ServiceInfo serviceInfo)
        {
            if (serviceInfo.Instance != null)
                return serviceInfo.Instance;

            object service;

            if (serviceInfo.Dependencies.Any())
            {
                List<object> dependencies = new List<object>();

                foreach (Type depType in serviceInfo.Dependencies)
                {
                    ASSERT_TRUE(k_services.ContainsKey(depType),
                        $"Missing service definition for {depType.Name}");

                    dependencies.Add(ConstructService(k_services[depType]));
                }

                service = Activator.CreateInstance(serviceInfo.Implementation, dependencies.ToArray(), new object[0]);
            }
            else
            {
                service = Activator.CreateInstance(serviceInfo.Implementation);
            }

            if (serviceInfo.IsSingleton)
                serviceInfo.Instance = service;

            return service;
        }
    }
}
