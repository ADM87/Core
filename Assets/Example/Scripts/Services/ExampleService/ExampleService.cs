using System.Collections.Generic;

namespace ADM.Example
{
    [ServiceDefinition(typeof(IExampleService), isSingleton: true)]
    internal class ExampleService : IExampleService
    {
        public IEnumerable<string> GetExampleNames()
        {
            return new string[] { "red", "blue", "yellow", "green", "purple" };
        }
    }
}
