namespace ADM
{
    public static class MessageTypes
    {
        public static void Register<T>(bool isSingleton = false)
            where T : IMessage
        {
            ServiceProvider.AddService<IMessenger<T>, Messenger<T>>(isSingleton);
        }
    }
}
