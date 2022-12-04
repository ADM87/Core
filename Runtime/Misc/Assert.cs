using UnityEngine;

namespace ADM.Core
{
    public static class Assert
    {
        public static void NotNull(object obj, string error, string objName)
        {
            if (obj == null || obj.Equals(default))
                throw new System.ArgumentNullException(objName, error);
        }

        //public static void NotNull(Object obj, string error)
        //{
        //    if (obj.Equals(null))
        //        throw new System.ArgumentNullException(nameof(obj), error);
        //}

        public static void That(bool condition, string error)
        {
            if (!condition)
                throw new System.Exception(error);
        }
    }
}