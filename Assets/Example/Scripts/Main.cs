using UnityEngine;

namespace ADM.Example
{
    internal static class Main
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void Boot()
        {
            // Registers the example message type. This will create a IMessenger<T>
            // service definition with the service provider that will allow 
            // sending and receiving messages of type ExampleMessage.
            // Flagging the message type as a singleton will ensure the IMessenger<T>
            // is the same instances anytime it is accessed from the ServiceProvider.
            MessageRegistry.RegisterMessageType<ExampleMessage>(isSingleton: true);

            // Collection definitions with the ServiceDefinition attribute.
            ServiceProvider.CollectDefinitions();
            
            // After all other definition registration is complete, 
            // we construst all service flagged as singletons.
            //
            // Note: This is optional depending on how you want to structure you systems.
            //       This ensures that any singleton services are ready to use before the
            //       rest of your game systems attempt to use them..
            ServiceProvider.ConstructSingletons();
        }
    }
}
