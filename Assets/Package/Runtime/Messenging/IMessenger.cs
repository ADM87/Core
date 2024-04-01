namespace ADM
{
    public interface IMessenger<T>
        where T : MessageBase
    {
        void AddListener(IMessageListener<T> listener);
        void RemoveListener(IMessageListener<T> listener);
        void Send(T message);
    }
}