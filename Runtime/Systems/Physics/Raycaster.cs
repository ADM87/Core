using UnityEngine;

namespace ADM.Core
{
    public static class Raycaster
    {
        public static bool GetWorldPoint(Ray ray, LayerMask layer, out Vector3 worldPoint)
        {
            worldPoint = Vector3.zero;
            if (Physics.Raycast(ray, out var hit, 100, layer))
            {
                worldPoint = hit.point;
                return true;
            }
            return false;
        }
    }
}
