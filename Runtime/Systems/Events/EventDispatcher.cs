using System;
using System.Collections.Generic;

namespace ADM.Core
{
    public static class EventDispatcher
    {
        // If no event names are provided, then the receiver will be notified of all events of the specified type.
        public const string RECEIVE_ALL = "<RECEIVE_ALL>";

        public class EventData
        {
            public readonly string Name;
            public EventData(string name)
                => Name = name;
        }

        private static readonly Dictionary<Type, Dictionary<string, HashSet<IEventReceiver>>> m_Receivers = new();

        public static void AddReceiver<T>(IEventReceiver<T> eventReceiver, params string[] eventNames)
            where T : EventData
        {
            Assert.NotNull(eventReceiver, "Cannot add a null event receiver");

            if (eventNames.Length == 0)
                eventNames = new[] { RECEIVE_ALL };

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
            if (m_Receivers.TryGetValue(typeof(T), out var eventReceivers))
            {
                Dispatch(RECEIVE_ALL, eventData, eventReceivers);
                Dispatch(eventData.Name, eventData, eventReceivers);
            }
        }

        private static void Dispatch<T>(string eventName, T eventData, Dictionary<string, HashSet<IEventReceiver>> eventReceivers)
            where T : EventData
        {
            if (eventReceivers.TryGetValue(eventName, out var receivers))
            {
                foreach (var receiver in receivers)
                    (receiver as IEventReceiver<T>).HandleEvent(in eventData);
            }
        }
    }
}
