using System;
using static ADM.Assert;

namespace ADM
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ServiceDefinitionAttribute : Attribute
    {
        public Type Interface { get; private set; }
        public bool IsSingleton { get; private set; }

        public ServiceDefinitionAttribute(Type interfaceType, bool isSingleton = false)
        {
            ASSERT_TRUE(interfaceType.IsInterface,
                $"{interfaceType.Name} must be an the interface of the service definition");

            Interface = interfaceType;
            IsSingleton = isSingleton;
        }
    }
}
