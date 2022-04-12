namespace Library.Structure
{
    public record Assembly
        ( Identifier Identifier
        , Reference<Project> Parent
        , Dictionary<Identifier, Namespace> Namespaces
        , Dictionary<Identifier, Assembly> References
        , Dictionary<Identifier, Attribute> Attributes
        ) : IEitherNamespaceOrAssembly, IIdentifiable
    {
        public System.Type Type_ =>
            typeof(Assembly);
    }
}
