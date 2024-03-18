using System;

namespace ADM
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ServiceDefinitionAttribute : Attribute
    {
        public Type Interface { get; private set; }
        public ServiceDefinitionAttribute(Type interfaceType)
            => Interface = interfaceType;
    }
}
