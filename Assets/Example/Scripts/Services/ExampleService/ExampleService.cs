using System.Collections.Generic;

namespace ADM.Example
{
    [ServiceDefinition(typeof(IExampleService), isSingleton: true)]
    internal class ExampleService : IExampleService
    {
        public void ProcessColorNames(IEnumerable<string> colorNames)
        {
            UnityEngine.Debug.Log(string.Join(", ", colorNames));
        }
    }
}
