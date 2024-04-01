using UnityEngine;

namespace ADM.Example
{
    internal class ExampleModule : MonoBehaviour
    {
        private void Awake()
        {
            if (ServiceProvider.TryGet(out IMessenger<ExampleMessage> messenger))
                messenger.Send(new ExampleMessage(new string[] { "red", "green", "blue" }));
        }

        private void Start()
        {
            if (ServiceProvider.TryGet(out IExampleService exampleService))
                Debug.Log(string.Join(", ", exampleService.GetColorNames()));
        }
    }
}
