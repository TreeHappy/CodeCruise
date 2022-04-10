namespace Library.Strcuture
{
    public record Assembly(Identifier Identifier, Dictionary<Identifier, Namespace> Namespaces, Dictionary<Identifier, Assembly> References, Dictionary<Identifier, Attribute> Attributes);
}