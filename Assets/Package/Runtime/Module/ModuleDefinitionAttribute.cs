using System;
using static ADM.Assert;

namespace ADM
{
    public class ModuleDefinitionAttribute : ServiceDefinitionAttribute
    {
        public static readonly Type k_moduleBase = typeof(ICoreModule);

        public Type InterfaceType { get; private set; }
        public ModuleDefinitionAttribute(Type interfaceType)
            : base(interfaceType, true)
        {
            ASSERT_TRUE(k_moduleBase.IsAssignableFrom(interfaceType), 
                $"{interfaceType.Name} must derive from {k_moduleBase.Name}");
        }
    }
}
