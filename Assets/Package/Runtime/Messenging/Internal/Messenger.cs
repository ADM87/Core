using System.Collections.Generic;

namespace ADM
{
    internal class Messenger<T> : IMessenger<T>
        where T : MessageBase
    {
        private HashSet<IMessageListener<T>> m_messageListeners = new();

        public void AddListener(IMessageListener<T> listener)
        {
            if (m_messageListeners.Contains(listener))
                return;

            m_messageListeners.Add(listener);
        }

        public void RemoveListener(IMessageListener<T> listener)
        {
            if (m_messageListeners.Contains(listener))
                m_messageListeners.Remove(listener);
        }

        public void Send(T message)
        {
            foreach (IMessageListener<T> listener in m_messageListeners)
                listener.HandleMessage(message);
        }
    }
}
