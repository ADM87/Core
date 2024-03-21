using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using static ADM.Assert;

namespace ADM
{
    public class ServiceProvider
    {
        private const string k_duplicateItrfType        = "Service interface type {0} already exists";
        private const string k_duplicateImplType        = "Service implementation type {0} already exists";
        private const string k_nonAbstractItrf          = "Service interface type {0} must be an interface";
        private const string k_abstractImpl             = "Service implementation {0} cannot be an abstract type";
        private const string k_invalidImplCstr          = "Service implementation {0} must contain only one constructor with dependencies";
        private const string k_circularDependency       = "Service implementation {0} detected a circular dependency on construction";
        private const string k_serviceNotDefined        = "Service implementation for {0} has not been defined";
        private const string k_doesNotDeriveFromImpl    = "Service implementation {0} from derive from {1}";

        private static Dictionary<Type, Type>       k_definitions   = new();
        private static Dictionary<Type, Type[]>     k_dependencies  = new();
        private static Dictionary<Type, object>     k_instances     = new();

        internal static void CollectServiceDefinitions()
        {
            IEnumerable<Assembly> assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var assembly in assemblies)
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (type.IsAbstract)
                        continue;

                    var attribute = type.GetCustomAttribute<ServiceDefinitionAttribute>(true);
                    if (attribute == null)
                        continue;

                    AddDefinition(attribute.Interface, type);
                }
            }
        }

        public static T Get<T>()
        {
            return (T)Get(typeof(T));
        }

        public static IEnumerable<T> GetAll<T>()
        {
            return k_definitions
                .Where(kvp => typeof(T).IsAssignableFrom(kvp.Key))
                .Select(kvp => (T)Get(kvp.Key));
        }

        public static object Get(Type type)
        {
            ASSERT_TRUE(k_definitions.ContainsKey(type), string.Format(k_serviceNotDefined, type.Name));

            if (k_instances.TryGetValue(type, out var instance))
                return instance;

            Type implType = k_definitions[type];

            foreach (Type depType in k_dependencies[implType])
            {
                ASSERT_TRUE(k_definitions.ContainsKey(depType), 
                    string.Format(k_serviceNotDefined, depType.Name));

                ASSERT_FALSE(IsCyclical(implType, k_definitions[depType]), 
                    string.Format(k_circularDependency, implType.Name));
            }

            return ConstructService(type, implType);
        }

        private static void AddDefinition(Type itrfType, Type implType)
        {
            ASSERT_FALSE(k_definitions.ContainsKey(itrfType), 
                string.Format(k_duplicateItrfType, itrfType.Name));

            ASSERT_FALSE(k_definitions.Any(kvp => kvp.Value.Equals(implType)), 
                string.Format(k_duplicateImplType, implType.Name));

            ASSERT_TRUE(itrfType.IsInterface,
                string.Format(k_nonAbstractItrf, itrfType.Name));

            ASSERT_TRUE(itrfType.IsAssignableFrom(implType),
                string.Format(k_doesNotDeriveFromImpl, implType.Name, itrfType.Name));

            ASSERT_TRUE(itrfType.IsAbstract, 
                string.Format(k_nonAbstractItrf, itrfType.Name));

            ASSERT_TRUE(!implType.IsAbstract, 
                string.Format(k_abstractImpl, implType.Name));

            k_definitions.Add(itrfType, implType);
            k_dependencies.Add(implType, GetImplementationDependencies(implType));
        }

        private static Type[] GetImplementationDependencies(Type implementationType)
        {
            return GetImplementationConstructor(implementationType)
                .GetParameters()
                .Select(info => info.ParameterType)
                .ToArray();
        }

        private static ConstructorInfo GetImplementationConstructor(Type implementationType)
        {
            var constructors = implementationType.GetConstructors();
            ASSERT_TRUE(constructors.Length == 1, string.Format(k_invalidImplCstr, implementationType.Name));
            return constructors[0];
        }

        private static bool IsCyclical(Type type, Type dependency)
        {
            if (type.Equals(dependency))
                return true;

            foreach (Type depType in k_dependencies[dependency])
            {
                if (IsCyclical(type, k_definitions[depType]))
                    return true;
            }

            return false;
        }

        private static object ConstructService(Type itrfType, Type implType)
        {
            if (k_instances.TryGetValue(itrfType, out var i))
                return i;

            object instance;
            if (k_dependencies[implType].Length == 0)
            {
                instance = Activator.CreateInstance(implType);
            }
            else
            {
                var dependencies = new List<object>();
                foreach (var depType in ServiceProvider.k_dependencies[implType])
                    dependencies.Add(ConstructService(depType, k_definitions[depType]));

                instance = Activator.CreateInstance(implType, dependencies.ToArray(), new object[0]);
            }

            k_instances.Add(itrfType, instance);
            return instance;
        }
    }
}
