namespace Library.Structure
{
    public record Namespace
        ( Identifier Identifier
        , Reference<IEitherNamespaceOrAssembly> Parent
        , Dictionary<Identifier, Namespace> Namespaces
        , Dictionary<Identifier, Type> Types
        ) : IEitherTypeOrNamespace, IEitherNamespaceOrAssembly, IIdentifiable
    {
        public System.Type Type_ =>
            typeof(Namespace);
    }
}
