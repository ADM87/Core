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
                $"{type.Name} must implement from {attribute.Interface.Name}");

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

        public static IEnumerable<T> GetAll<T>()
        {
            return k_services
                .Where(kvp => typeof(T).IsAssignableFrom(kvp.Key))
                .Select(kvp => (T)Get(kvp.Key));
        }

        public static object Get(Type type)
        {
            ServiceInfo serviceInfo = GetServiceInfo(type);

            if (serviceInfo.SingletonInstance != null)
                return serviceInfo.SingletonInstance;

            if (serviceInfo.CheckDependencies)
                CheckDependencies(serviceInfo);

            return ConstructService(serviceInfo);
        }

        private static ServiceInfo GetServiceInfo(Type type)
        {
            ASSERT_TRUE(type.IsInterface,
                $"Service interface required for accessing service definition information, [{type.Name}]");

            ASSERT_TRUE(k_services.ContainsKey(type),
                $"{type.Name} is not a known service interface. Are you missing a ServiceDefinition attribute?");

            return k_services[type];
        }

        private static void CheckDependencies(ServiceInfo serviceInfo)
        {
            foreach (Type dependency in serviceInfo.Dependencies)
            {
                ASSERT_FALSE(serviceInfo.Interface.Equals(dependency),
                    $"Service {serviceInfo.Interface.Name} cannot have a constructor dependency of {dependency.Name}");

                ServiceInfo dependencyInfo = GetServiceInfo(dependency);

                ASSERT_FALSE(IsCircular(serviceInfo, dependencyInfo),
                    $"Detected circular dependencys for {serviceInfo.Interface.Name}, {dependency.Name}");

                if (dependencyInfo.CheckDependencies)
                    CheckDependencies(dependencyInfo);
            }
            serviceInfo.CheckDependencies = false;
        }

        private static bool IsCircular(ServiceInfo serviceInfo, ServiceInfo dependencyInfo)
        {
            foreach (Type dependency in dependencyInfo.Dependencies)
            {
                if (IsCircular(serviceInfo, GetServiceInfo(dependency)))
                    return true;
            }
            return false;
        }

        private static object ConstructService(ServiceInfo serviceInfo)
        {
            if (serviceInfo.SingletonInstance != null)
                return serviceInfo.SingletonInstance;

            object instance;

            if (serviceInfo.Dependencies.Any())
            {
                List<object> dependencies = new List<object>();

                foreach (Type depType in serviceInfo.Dependencies)
                {
                    ASSERT_TRUE(k_services.ContainsKey(depType),
                        $"Missing service definition for {depType.Name}");

                    dependencies.Add(ConstructService(k_services[depType]));
                }

                instance = Activator.CreateInstance(serviceInfo.Implementation, dependencies.ToArray(), new object[0]);
            }
            else
            {
                instance = Activator.CreateInstance(serviceInfo.Implementation);
            }

            if (serviceInfo.IsSingleton)
                serviceInfo.SingletonInstance = instance;

            return instance;
        }
    }
}
