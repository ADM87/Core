namespace ADM
{
    public abstract class EventBase
    {
        public string Name { get; private set; }
        public EventBase(string name)
            => Name = name;
    }
}