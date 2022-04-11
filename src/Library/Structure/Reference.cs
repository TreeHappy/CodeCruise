namespace Library.Structure
{
    public sealed class Reference<T> where T : IIdentifiable
    {
        public Identifier Identifier { get; private set; }
        public System.Type Type { get; private set; }

        public Reference(Identifier identifier)
        {
            Identifier = identifier;
        }

        public Reference(T value)
        {
            Identifier = value.Identifier;
            Type = typeof(T);
        }

        public void SetIdentifier(T value)
        {
            Identifier = value.Identifier;
        }
    }
}