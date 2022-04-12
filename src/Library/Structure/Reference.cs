namespace Library.Structure
{
    public sealed class Reference<T> where T : IIdentifiable
    {
        public Identifier Identifier { get; private set; }
        public string TypeName { get; private set; }

        public Reference(Identifier identifier, System.Type type)
        {
            Identifier = identifier;
            TypeName = type.FullName;
        }

        public Reference(T value) : this(value.Identifier, value.Type_) {}

        public void SetIdentifier(T value)
        {
            Identifier = value.Identifier;
        }
    }
}