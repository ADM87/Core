using System;
using System.Reflection;
using static ADM.Assert;

namespace ADM
{
    public static class ServiceInjector
    {
        public static void InjectDependencies(object target)
        {
            Type targetType = target.GetType();

            foreach (PropertyInfo property in targetType.GetProperties())
            {
                if (property.GetCustomAttribute<ServiceDependencyAttribute>() == null)
                    continue;

                ASSERT_TRUE(property.GetSetMethod(true) != null,
                    $"Property {property.Name} on {targetType.Name} is missing a private set accessor method.");

                ASSERT_TRUE(property.PropertyType.IsInterface,
                    $"Property type {property.PropertyType.Name} must be an interface type.");

                property.SetValue(target, ServiceProvider.Get(property.PropertyType));
            }
        }
    }
}
