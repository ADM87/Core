using System.Collections.Generic;

namespace ADM
{
    internal class EventDispatcher<T> : IEventDispatcher<T>
        where T : EventBase
    {
        private HashSet<IEventListener<T>> m_eventListeners = new();

        public void AddListener(IEventListener<T> eventListener)
        {
            if (m_eventListeners.Contains(eventListener))
                return;

            m_eventListeners.Add(eventListener);
        }

        public void RemoveListener(IEventListener<T> eventListener)
        {
            if (m_eventListeners.Contains(eventListener))
                m_eventListeners.Remove(eventListener);
        }

        public void Send(T @event)
        {
            foreach (IEventListener<T> eventListener in m_eventListeners)
                eventListener.HandleEvent(@event);
        }
    }
}
