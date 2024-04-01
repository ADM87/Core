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

            // Register the example message type. This will create a IMessenger<T>
            // service definition with the service provider that will allow 
            // sending and receiving messages of type ExampleMessage.
            MessageRegistry.RegisterMessageType<ExampleMessage>(isSingleton: true);

            // After all other definition registration is complete, 
            // we construst all singleton service instances.
            //
            // Note: This is optional depending on how you want to structure you systems.
            //       This ensures that any singleton services are ready to use before the
            //       rest of your game systems attempt to use them..
            ServiceProvider.ConstructSingletons();
        }
    }
}
