using System;
using static Core.Assert;

namespace Core
{
    public class ModuleDefinitionAttribute : ServiceDefinitionAttribute
    {
        public static readonly Type k_moduleBase = typeof(ICoreModule);

        public Type InterfaceType { get; private set; }
        public ModuleDefinitionAttribute(Type interfaceType)
            : base(interfaceType)
        {
            ASSERT_TRUE(k_moduleBase.IsAssignableFrom(interfaceType), $"Module {interfaceType.Name} must derive from {k_moduleBase.Name}");
        }
    }
}
