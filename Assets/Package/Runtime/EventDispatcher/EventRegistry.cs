namespace ADM
{
    public static class EventRegistry
    {
        public static void AddEventType<T>(bool isSingleton = false)
            where T : EventBase
        {
            ServiceProvider.AddService<IEventDispatcher<T>, EventDispatcher<T>>(isSingleton);
        }
    }
}
