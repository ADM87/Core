namespace ADM
{
    public static class MessageRegistry
    {
        public static void RegisterMessageType<T>(bool isSingleton = false)
            where T : MessageBase
        {
            ServiceProvider.AddService<IMessenger<T>, Messenger<T>>(isSingleton);
        }
    }
}
