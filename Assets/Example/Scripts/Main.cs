using UnityEngine;

namespace ADM.Example
{
    internal static class Main
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void Boot()
        {
            // Collection definitions with the ServiceDefinition attribute.
            ServiceProvider.CollectDefinitions();

            // Register event types to create IEventDispatcher<T> definitions.
            EventRegistry.AddEventType<ExampleEvent>(isSingleton: true);
        }
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Start()
        {
            // Get the event dispatcher to the registered ExampleEvent.
            IEventDispatcher<ExampleEvent> eventDispatcher = ServiceProvider.Get<IEventDispatcher<ExampleEvent>>();

            // Use the event dispatcher service to send events.
            eventDispatcher.Send(new ExampleEvent(new string[] { "red", "yellow", "green", "blue" }));
        }
    }
}
