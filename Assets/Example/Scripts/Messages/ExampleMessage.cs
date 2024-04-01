using System.Collections.Generic;

namespace ADM.Example
{
    public class ExampleMessage : MessageBase
    {
        public IEnumerable<string> ColorNames { get; private set; }

        public ExampleMessage(IEnumerable<string> colorNames)
            : base("exampleMessage")
        {
            ColorNames = colorNames;
        }
    }
}