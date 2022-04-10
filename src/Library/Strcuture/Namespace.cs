namespace Library.Strcuture
{
    public record Namespace(Identifier Identifier, Dictionary<Identifier, Namespace> Namespaces, Dictionary<Identifier, Type> Types);
}