using ADM;

namespace CoreExample
{
    [ModuleDefinition(typeof(IExampleModule))]
    internal class ExampleModule : IExampleModule
    {
        public void Load()
        {
            throw new System.NotImplementedException();
        }
    }
}
