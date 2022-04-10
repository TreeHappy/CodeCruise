namespace Library.Strcuture
{
    public static class Conversion
    {
        public static Accessibility From(Microsoft.CodeAnalysis.Accessibility accessability) =>
            accessability switch
            { Microsoft.CodeAnalysis.Accessibility.NotApplicable => Accessibility.NotApplicable
            , Microsoft.CodeAnalysis.Accessibility.Private => Accessibility.Private
            , Microsoft.CodeAnalysis.Accessibility.ProtectedAndInternal => Accessibility.ProtectedAndInternal
            //, Microsoft.CodeAnalysis.Accessibility.ProtectedAndFriend => Accessibility.ProtectedAndFriend
            , Microsoft.CodeAnalysis.Accessibility.Protected => Accessibility.Protected
            , Microsoft.CodeAnalysis.Accessibility.Internal => Accessibility.Internal
            //, Microsoft.CodeAnalysis.Accessibility.Friend => Accessibility.Friend
            , Microsoft.CodeAnalysis.Accessibility.ProtectedOrInternal => Accessibility.ProtectedOrInternal
            // , Microsoft.CodeAnalysis.Accessibility.ProtectedOrFriend => Accessibility.ProtectedOrFriend
            , Microsoft.CodeAnalysis.Accessibility.Public => Accessibility.Public
            };
    }
}
