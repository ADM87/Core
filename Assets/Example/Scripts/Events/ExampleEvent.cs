using System.Collections.Generic;

namespace ADM.Example
{
    public class ExampleEvent : EventBase
    {
        public IEnumerable<string> ColorNames { get; private set; }

        public ExampleEvent(IEnumerable<string> colorNames)
            : base("exampleEvent")
        {
            ColorNames = colorNames;
        }
    }
}