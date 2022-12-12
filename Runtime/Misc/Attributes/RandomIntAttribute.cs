using UnityEngine;

namespace ADM.Core
{
    public class RandomIntAttribute : PropertyAttribute
    {
        public readonly int Min;
        public readonly int Max;
        public RandomIntAttribute(int min, int max)
        {
            Min = min;
            Max = max;
        }
    }
}
