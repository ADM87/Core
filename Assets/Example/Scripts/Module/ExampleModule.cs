using UnityEngine;

namespace ADM.Example
{
    internal class ExampleModule : MonoBehaviour
    {
        private void Awake()
        {
            if (ServiceProvider.TryGet(out IEventDispatcher<ExampleEvent> exampleEvents))
                exampleEvents.Send(new ExampleEvent(new string[] { "red", "green", "blue" }));
        }

        private void Start()
        {
            if (ServiceProvider.TryGet(out IExampleService exampleService))
                Debug.Log(string.Join(", ", exampleService.GetColorNames()));
        }
    }
}
