namespace ADM.Core
{
    public interface IEventReceiver { }
    public interface IEventReceiver<TEventData> : IEventReceiver
        where TEventData : EventService.EventData
    {
        void HandleEvent(in TEventData eventData);
    }
}
