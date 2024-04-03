namespace ADM
{
    public static class Messages
    {
        public static void RegisterMessageType<T>(bool isSingleton = false)
            where T : IMessage
        {
            ServiceProvider.AddService<IMessenger<T>, Messenger<T>>(isSingleton);
        }
    }
}
