namespace ADM.Core
{
    public static class Assert
    {
        public static void NotNull(object obj, string error, string objName)
        {
            if (obj == null || obj.Equals(null))
                throw new System.ArgumentNullException(objName, error);
        }

        public static void That(bool condition, string error)
        {
            if (!condition)
                throw new System.Exception(error);
        }
    }
}