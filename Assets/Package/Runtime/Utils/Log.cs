namespace ADM
{
    public static class Log
    {
        public static void LOG_DEBUG(string message)
            => UnityEngine.Debug.Log(message);

        public static void LOG_WARNING(string message)
            => UnityEngine.Debug.LogWarning(message);

        public static void LOG_ERROR(string message)
            => UnityEngine.Debug.LogError(message);

        public static void LOG_DEBUG<T>(T sender, string message)
            where T : class
        {
            if (sender == null)
                LOG_DEBUG(message);
            else
                LOG_DEBUG(string.Format("{0} {1}", sender.GetType().Name, message));
        }

        public static void LOG_WARNING<T>(T sender, string message)
            where T : class
        {
            if (sender == null)
                LOG_WARNING(message);
            else
                LOG_WARNING(string.Format("{0} {1}", sender.GetType().Name, message));
        }

        public static void LOG_ERROR<T>(T sender, string message)
            where T : class
        {
            if (sender == null)
                LOG_ERROR(message);
            else
                LOG_ERROR(string.Format("{0} {1}", sender.GetType().Name, message));
        }
    }
}
