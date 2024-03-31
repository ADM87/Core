namespace ADM
{
    public interface IEventListener<T>
        where T : EventBase
    {
        void HandleEvent(T @event);
    }
}