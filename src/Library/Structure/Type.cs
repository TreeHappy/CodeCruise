namespace Library.Structure
{

    public record Type
        ( Identifier Identifier
        , Reference<IEitherTypeOrNamespace> Parent
        , Accessibility AccessModifier
        , Dictionary<Identifier, Method> Methods
        , Dictionary<Identifier, Property> Properties
        , Dictionary<Identifier, Field> Fields
        , Dictionary<Identifier, Reference<Structure.Attribute>> Attributes
        ) : IEitherTypeOrNamespace, IIdentifiable
    {
        public System.Type Type_ =>
            typeof(Type);
    }
}