using ADM;
using UnityEngine;

namespace CoreExample
{
    internal static class Main
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void OnAfterAssembliesLoaded()
            => ADMCore.StartSystems();
    }
}
