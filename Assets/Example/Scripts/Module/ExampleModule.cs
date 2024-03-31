using UnityEngine;

namespace ADM.Example
{
    internal class ExampleModule : MonoBehaviour, IEventListener<ExampleEvent>
    {
        private void Awake()
        {
            if (ServiceProvider.TryGet(out IEventDispatcher<ExampleEvent> eventDispatcher))
                eventDispatcher.AddListener(this);
        }

        public void HandleEvent(ExampleEvent @event)
        {
            if (ServiceProvider.TryGet(out IExampleService exampleService))
                exampleService.ProcessColorNames(@event.ColorNames);
        }
    }
}
