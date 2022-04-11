namespace Library.Structure
{
    public record Method
        ( Identifier Identifier
        , Reference<Type> ReturnType
        , Dictionary<Identifier, Reference<Type>> Parameters
        , Dictionary<Identifier, Reference<Type>> UsedInBody
        );
}
