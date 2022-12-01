using System;
using UnityEngine;

namespace ADM.Core
{
    public static class Find
    {
        public static void Required<TComponent>(out TComponent component) where TComponent : Component
            => Assert.NotNull(component = UnityEngine.Object.FindObjectOfType<TComponent>(), $"Unable to find required component {typeof(TComponent).Name}");

        public static FindComponent<TComponent> Required<TComponent>(Action<TComponent> onFound, float searchIntervals = 0.25f) where TComponent : Component
            => new(onFound, searchIntervals);

        public static bool Component<TComponent>(out TComponent component) where TComponent : Component
            => !(component = UnityEngine.Object.FindObjectOfType<TComponent>()).Equals(null);

        public static string HierarchyName(GameObject gameObject)
        {
            var name = gameObject.name;
            var parent = gameObject.transform.parent;

            while (parent != null)
            {
                name = $"{parent.name}.{name}";
                parent = parent.parent;
            }

            return name;
        }
    }
}
