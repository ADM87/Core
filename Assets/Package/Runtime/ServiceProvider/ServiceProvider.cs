using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using static Core.Assert;

namespace Core
{
    public class ServiceProvider
    {
        private const string k_duplicateItrfType    = "Service interface type {0} already exists";
        private const string k_duplicateImplType    = "Service implementation type {0} already exists";
        private const string k_nonAbstractItrf      = "Service interface type {0} must be an abstract type";
        private const string k_abstractImpl         = "Service implementation {0} cannot be an abstract type";
        private const string k_invalidImplCstr      = "Service implementation {0} must contain only one constructor with dependencies";
        private const string k_circularDependency   = "Service implementation {0} detected a circular dependency on construction";
        private const string k_serviceNotDefined    = "Service implementation for {0} has not been defined";

        internal static Dictionary<Type, Type>           definitions   = new();
        internal static Dictionary<Type, Type[]>         dependencies  = new();
        internal static Dictionary<Type, object>         instances     = new();

        public static void ConstructServices()
        {
            CollectServiceDefinitions();
            ValidateDependencies();
            foreach (var kvp in definitions)
                ConstructService(kvp.Key, kvp.Value);
        }

        public static T Get<T>()
        {
            Type type = typeof(T);
            ASSERT_TRUE(instances.ContainsKey(type), string.Format(k_serviceNotDefined, type.Name));
            return (T)instances[type];
        }

        public static IEnumerable<T> GetAll<T>()
        {
            return Enumerable.Empty<T>();
        }

        private static void CollectServiceDefinitions()
        {

        }

        private static void ValidateDependencies()
        {
            foreach (Type implType in definitions.Values)
            {
                foreach (Type depType in dependencies[implType])
                    ASSERT_FALSE(IsCyclical(implType, definitions[depType]), string.Format(k_circularDependency, implType.Name));
            }
        }

        private static void AddDefinition(Type itrfType, Type implType)
        {
            ASSERT_FALSE(definitions.ContainsKey(itrfType), 
                string.Format(k_duplicateItrfType, itrfType.Name));

            ASSERT_FALSE(definitions.Any(kvp => kvp.Value.Equals(implType)), 
                string.Format(k_duplicateImplType, implType.Name));

            ASSERT_TRUE(itrfType.IsAbstract, 
                string.Format(k_nonAbstractItrf, itrfType.Name));

            ASSERT_TRUE(!implType.IsAbstract, 
                string.Format(k_abstractImpl, implType.Name));

            definitions.Add(itrfType, implType);
            dependencies.Add(implType, GetImplementationDependencies(implType));
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

            foreach (Type depType in dependencies[dependency])
            {
                if (IsCyclical(type, definitions[depType]))
                    return true;
            }

            return false;
        }

        private static object ConstructService(Type itrfType, Type implType)
        {
            if (instances.TryGetValue(itrfType, out var i))
                return i;

            object instance;
            if (dependencies[implType].Length == 0)
            {
                instance = Activator.CreateInstance(implType);
            }
            else
            {
                var dependencies = new List<object>();
                foreach (var depType in ServiceProvider.dependencies[implType])
                    dependencies.Add(ConstructService(depType, definitions[depType]));

                instance = Activator.CreateInstance(implType, dependencies.ToArray(), new object[0]);
            }

            instances.Add(itrfType, instance);
            return instance;
        }
    }
}
