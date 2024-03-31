namespace ADM
{
    public interface IEventDispatcher<T>
        where T : EventBase
    {
        void AddListener(IEventListener<T> eventListener);
        void RemoveListener(IEventListener<T> eventListener);
        void Send(T @event);
    }
}