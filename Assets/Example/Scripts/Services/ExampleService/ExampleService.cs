using System.Collections.Generic;

namespace ADM.Example
{
    [ServiceDefinition(typeof(IExampleService), isSingleton: true)]
    internal class ExampleService : IExampleService, IMessageListener<ExampleMessage>
    {
        private IEnumerable<string> m_colorNames;

        // Services will be injects with their dependencies upon construction.
        // Be sure to understand your dependency tree, as circular dependencies
        // are not allow and will result in an error being thrown.
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
