using UnityEngine;

namespace ADM.Example
{
    internal static class Main
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void Boot()
        {
            // Register a message type.
            // This will create a service definition for an IMessenger<T> which can be
            // accessed through the ServiceProvider.
            // Use the IMessenger<T> to send and receive messages of a given type.
            // Setting a message type as a singleton will cause the same IMessenger<T>
            // service instance to be used for the given message type.
            Messages.RegisterMessageType<ExampleMessage>(isSingleton: true);

            // Collection definitions with the ServiceDefinition attribute.
            ServiceProvider.CollectDefinitions();

            // After all other definition registration is complete,
            // we construst all service flagged as singletons.
            //
            // Note - This call is optional. If you have singleton services that you want
            //        to exist right away, calling this will construct those services and
            //        and of their dependencies. Remember, this will applies to all singleton services.
            ServiceProvider.ConstructSingletons();
        }
    }
}
