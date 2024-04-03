using System.Threading;
using System.Threading.Tasks;
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

            if (ServiceProvider.TryGet(out IAsyncService asyncService))
            {
                asyncService.RunAsync(async (CancellationToken ct) =>
                {
                    await Task.Delay(5);
                    Debug.Log("Log 1");
                });

                Debug.Log("Log 2");

                asyncService.RunAsync(DoSomethingAsync);
            }
        }

        private async Task DoSomethingAsync(CancellationToken ct)
        {
            await Task.Delay(10);
            Debug.Log("Log 3");
        }
    }
}
