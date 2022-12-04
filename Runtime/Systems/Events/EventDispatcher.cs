using System;
using System.Collections.Generic;
using System.Linq;

namespace ADM.Core
{
    public static class EventDispatcher
    {
        public class EventData
        {
            public readonly string Name;
            public EventData(string name)
                => Name = name;
        }

        private static Dictionary<Type, Dictionary<IEventReceiver, HashSet<string>>> m_EventReceivers = new();

        public static void AddReceiver<T>(IEventReceiver<T> eventReceiver, params string[] eventNames)
            where T : EventData
        {
            Assert.NotNull(eventReceiver, "Cannot add a null event receiver", nameof(eventReceiver));

            var eventType = typeof(T);

            if (m_EventReceivers.TryGetValue(eventType, out var receivers))
            {
                if (receivers.ContainsKey(eventReceiver))
                    return;

                receivers.Add(eventReceiver, eventNames.ToHashSet());
            }
            else
            {
                m_EventReceivers[eventType] = new Dictionary<IEventReceiver, HashSet<string>>
                {
                    { eventReceiver, eventNames.ToHashSet() }
                };
            }
        }

        public static void RemoveReceiver<T>(IEventReceiver<T> eventReceiver)
            where T : EventData
        {
            Assert.NotNull(eventReceiver, "Cannot remove a null event receiver", nameof(eventReceiver));

            var eventType = typeof(T);

            if (!m_EventReceivers.TryGetValue(eventType, out var receivers))
                return;

            receivers.Remove(eventReceiver);

            if (receivers.Count == 0)
                m_EventReceivers.Remove(eventType);
        }

        public static void Dispatch<T>(T eventData)
            where T : EventData
        {
            var eventType = typeof(T);

            if (!m_EventReceivers.TryGetValue(eventType, out var receivers))
                return;

            receivers.Keys
                .ToArray()
                .ForEach(receiver =>
                {
                    // Make sure a previous receiver's event stack hasn't removed this receiver from the system.
                    if (!m_EventReceivers.TryGetValue(eventType, out var rs) || !rs.TryGetValue(receiver, out var eventNames))
                        return;

                    if (eventNames.Count == 0 || eventNames.Contains(eventData.Name))
                        (receiver as IEventReceiver<T>).HandleEvent(eventData);
                });
        }
    }
}
