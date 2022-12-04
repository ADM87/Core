using UnityEngine;

namespace ADM.Core
{
    public static class VectorExtensions
    {
        public static Vector2Int FloorToInt(this Vector2 vector)
            => new(Mathf.FloorToInt(vector.x), 
                   Mathf.FloorToInt(vector.y));

        public static Vector2Int CeilToInt(this Vector2 vector)
            => new(Mathf.CeilToInt(vector.x),
                   Mathf.CeilToInt(vector.y));
    }
}
