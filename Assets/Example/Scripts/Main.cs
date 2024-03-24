using UnityEngine;

namespace ADM.Example
{
    internal static class Main
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void OnAfterAssembliesLoaded()
            => ADMCore.StartSystems();
    }
}
