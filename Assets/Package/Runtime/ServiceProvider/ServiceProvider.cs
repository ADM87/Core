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

        public static void CollectDefinitions()
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

                    AddService(attribute.Interface, type, attribute.IsSingleton);
                }
            }
        }

        public static void AddService<TInterface, TImplementation>(bool isSingleton = false) 
            where TImplementation : TInterface
        {
            AddService(typeof(TInterface), typeof(TImplementation), isSingleton);
        }
        
        public static void AddService(Type anInterface, Type anImplementation, bool isSingleton)
        {
            ASSERT_FALSE(k_services.ContainsKey(anInterface),
                $"Definition for {anInterface.Name} already exists");

            ASSERT_FALSE(anImplementation.IsAbstract,
                $"Implementation {anImplementation.Name} cannot be an abstract type");

            ASSERT_FALSE(k_services.Values.Any(service => service.Implementation.Equals(anImplementation)),
                $"Implementation {anImplementation.Name} already exists for another service");

            ASSERT_TRUE(anInterface.IsAssignableFrom(anImplementation),
                $"{anImplementation.Name} must implement from {anInterface.Name}");

            k_services.Add(anInterface, new ServiceInfo
            {
                Interface       = anInterface,
                Implementation  = anImplementation,
                Dependencies    = GetDependencies(anImplementation),
                IsSingleton     = isSingleton
            });
        }

        public static void ConstructSingletons()
        {
            foreach (var kvp in k_services)
            {
                if (kvp.Value.IsSingleton)
                    Get(kvp.Key);
            }
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

        public static bool TryGet<T>(out T service)
        {
            Type serviceType = typeof(T);

            if (k_services.ContainsKey(serviceType))
            {
                service = (T)Get(serviceType);
                return true;
            }

            service = default;
            return false;
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
            if (serviceInfo.Interface.Equals(dependencyInfo.Interface))
                return true;
            
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

            List<object> dependencies = new List<object>();
            object service = null;

            foreach (Type dependency in serviceInfo.Dependencies)
                dependencies.Add(ConstructService(GetServiceInfo(dependency)));
                                     
            service = Activator.CreateInstance(serviceInfo.Implementation, dependencies.ToArray(), new object[0]);

            if (serviceInfo.IsSingleton)
                serviceInfo.SingletonInstance = service;

            return service;
        }
    }
}
