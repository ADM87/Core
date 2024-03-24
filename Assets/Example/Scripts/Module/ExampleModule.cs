using static ADM.Log;

namespace ADM.Example
{
    [ModuleDefinition(typeof(IExampleModule))]
    internal class ExampleModule : IExampleModule
    {
        private readonly IExampleService m_exampleService;

        public ExampleModule(IExampleService exampleService)
        {
            m_exampleService = exampleService;
        }

        public void Load()
        {
            LOG_DEBUG(string.Join(", ", m_exampleService.GetExampleNames()));
        }
    }
}
