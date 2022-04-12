namespace Library.Structure
{
    public record Project(Identifier Identifier, Assembly Assembly) : IIdentifiable
    {
        public System.Type Type_ =>
            typeof(Project);
    }
}
