namespace Library.Strcuture
{
    public record Method(Identifier Identifier, TypeReference ReturnType, Dictionary<Identifier, TypeReference> Parameters, Dictionary<Identifier, TypeReference> UsedInBody);
}