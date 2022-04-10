namespace Library.Strcuture
{
    public record Type(Identifier Identifier, Accessibility AccessModifier, Dictionary<Identifier, Method> Methods, Dictionary<Identifier, Property> Properties, Dictionary<Identifier, Field> Fields, Dictionary<Identifier, Attibute> Attibutes);
}