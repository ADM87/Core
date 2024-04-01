namespace ADM
{
    public interface IMessageListener<T>
        where T : MessageBase
    {
        void HandleMessage(T message);
    }
}