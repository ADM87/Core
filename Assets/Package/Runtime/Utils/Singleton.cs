namespace Core
{
    public abstract class Singleton<T> where T : Singleton<T>
    {
        private static T k_instance;

        public static T GetInstance()
            => k_instance ??= System.Activator.CreateInstance<T>();
    }
}
