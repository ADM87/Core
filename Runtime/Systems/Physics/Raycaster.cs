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

        public static bool ScanFor<T>(Ray ray, LayerMask layer, out T scannedObject)
            where T : Component
        {
            if (Physics.Raycast(ray, out var hit, 100, layer))
                return (scannedObject = hit.collider.GetComponent<T>()) != null;

            scannedObject = default;
            return false;
        }
    }
}
