using System.Collections.Generic;

namespace ADM.Example
{
    [ServiceDefinition(typeof(IExampleService), isSingleton: true)]
    internal class ExampleService : IExampleService, IMessageListener<ExampleMessage>
    {
        private IEnumerable<string> m_colorNames;

        // Services will be injected with their dependencies upon construction.
        // Circlar dependencies are not allow on construction of a service, 
        // but are allowed outside of the constructor.
        public ExampleService(IMessenger<ExampleMessage> messenger)
        {
            messenger.AddListener(this);
        }

        public IEnumerable<string> GetColorNames()
        {
            return m_colorNames;
        }

        public void HandleMessage(ExampleMessage message)
        {
            m_colorNames = message.ColorNames;
        }
    }
}
