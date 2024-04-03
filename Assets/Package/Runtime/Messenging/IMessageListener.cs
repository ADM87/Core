namespace ADM
{
    public interface IMessageListener<T>
        where T : IMessage
    {
        void HandleMessage(T message);
    }
}