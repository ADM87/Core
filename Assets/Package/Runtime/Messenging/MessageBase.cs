namespace ADM
{
    public abstract class MessageBase
    {
        public string Name { get; private set; }
        public MessageBase(string name)
            => Name = name;
    }
}