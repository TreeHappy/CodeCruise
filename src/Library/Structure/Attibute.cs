namespace Library.Structure
{
    public record Attribute(Identifier Identifier) : IIdentifiable
    {
        public System.Type Type_ =>
            typeof(Attribute);
    }
}
