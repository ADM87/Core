using System;
using System.Collections.Generic;

namespace ADM.Core
{
    public static class EventDispatcher
    {
        public class EventData
        {
            public string Name { get; private set; }
            public EventData(string name)
                => Name = name;
        }

        private static readonly Dictionary<Type, Dictionary<string, HashSet<IEventReceiver>>> m_Receivers = new();

        public static void AddReceiver<T>(IEventReceiver<T> eventReceiver, params string[] eventNames)
            where T : EventData
        {
            Assert.NotNull(eventReceiver, "Cannot add a null event receiver");
            Assert.That(eventNames.Length > 0, "Must provide the name of events that the receiver will handle");

            var eventType = typeof(T);
            if (!m_Receivers.TryGetValue(eventType, out var eventReceivers))
                eventReceivers = new Dictionary<string, HashSet<IEventReceiver>>();

            foreach (var name in eventNames)
            {
                if (!eventReceivers.TryGetValue(name, out var receivers))
                    receivers = new HashSet<IEventReceiver>();

                if (receivers.Contains(eventReceiver))
                    continue;

                receivers.Add(eventReceiver);
                eventReceivers[name] = receivers;
            }

            if (eventReceivers.Count > 0)
                m_Receivers[eventType] = eventReceivers;
        }

        public static void RemoveReceiver<T>(IEventReceiver<T> eventReceiver)
            where T : EventData
        {
            Assert.NotNull(eventReceiver, "Cannot remove a null event receiver");

            var eventType = typeof(T);
            if (m_Receivers.TryGetValue(eventType, out var eventReceivers))
            {
                var eventNames = new List<string>(eventReceivers.Keys);
                foreach (var eventName in eventNames)
                {
                    eventReceivers[eventName].Remove(eventReceiver);
                    if (eventReceivers[eventName].Count == 0)
                        eventReceivers.Remove(eventName);
                }

                if (eventReceivers.Count == 0)
                    m_Receivers.Remove(eventType);
            }
        }

        public static void Dispatch<T>(T eventData)
            where T : EventData 
        {
            if (m_Receivers.TryGetValue(typeof(T), out var eventReceivers) && 
                eventReceivers.TryGetValue(eventData.Name, out var receivers))
            {
                foreach (var receiver in receivers)
                    (receiver as IEventReceiver<T>).HandleEvent(in eventData);
            }
        }
    }
}
