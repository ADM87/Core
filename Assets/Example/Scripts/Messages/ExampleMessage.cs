using System.Collections.Generic;

namespace ADM.Example
{
    public class ExampleMessage : IMessage
    {
        public string MessageName { get; private set; } = "ExampleMessage";

        public IEnumerable<string> ColorNames { get; private set; }

        public ExampleMessage(IEnumerable<string> colorNames)
        {
            ColorNames = colorNames;
        }
    }
}
