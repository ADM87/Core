using System;

namespace ADM
{
    public static class Assert
    {
        public delegate string GetAssertDetail();

        public static void ASSERT_TRUE(bool condition, string getMsg = null)
        {
            if (!condition)
                throw new Exception(getMsg ?? "Condition was false");
        }

        public static void ASSERT_FALSE(bool condition, string getMsg = null)
        {
            if (condition)
                throw new Exception(getMsg ?? "Condition was true");
        }

        public static void ASSERT_NOT_NULL<T>(T obj, string objName, string message = null)
            where T : class
        {
            if (obj == null || obj.Equals(null))
                throw new ArgumentNullException(objName, message ?? "Object was null");
        }

        public static void ASSERT_IS_NULL<T>(T obj, string objName, string message = null)
            where T : class
        {
            if (obj != null || !obj.Equals(null))
                throw new ArgumentNullException(objName, message ?? "Object was not null");
        }
    }
}
