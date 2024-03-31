using System.Collections.Generic;

namespace ADM.Example
{
    [ServiceDefinition(typeof(IExampleService), isSingleton: true)]
    internal class ExampleService : IExampleService, IEventListener<ExampleEvent>
    {
        private IEnumerable<string> m_colorNames;

        // Services will be injects with their dependencies upon construction.
        // Be sure to understand your dependency tree, as circular dependencies
        // are not allow and will result in an error being thrown.
        public ExampleService(IEventDispatcher<ExampleEvent> exampleEvents)
        {
            exampleEvents.AddListener(this);
        }

        public IEnumerable<string> GetColorNames()
        {
            return m_colorNames;
        }

        public void HandleEvent(ExampleEvent @event)
        {
            m_colorNames = @event.ColorNames;
        }
    }
}
