namespace Library.Structure
{
    public enum Accessibility
    {
        NotApplicable = 0,
        Private = 1,
        //
        // Summary:
        //     Only accessible where both protected and internal members are accessible (more
        //     restrictive than Microsoft.CodeAnalysis.Accessibility.Protected, Microsoft.CodeAnalysis.Accessibility.Internal
        //     and Microsoft.CodeAnalysis.Accessibility.ProtectedOrInternal).
        ProtectedAndInternal = 2,
        //
        // Summary:
        //     Only accessible where both protected and friend members are accessible (more
        //     restrictive than Microsoft.CodeAnalysis.Accessibility.Protected, Microsoft.CodeAnalysis.Accessibility.Friend
        //     and Microsoft.CodeAnalysis.Accessibility.ProtectedOrFriend).
        ProtectedAndFriend = 2,
        Protected = 3,
        Internal = 4,
        Friend = 4,
        //
        // Summary:
        //     Accessible wherever either protected or internal members are accessible (less
        //     restrictive than Microsoft.CodeAnalysis.Accessibility.Protected, Microsoft.CodeAnalysis.Accessibility.Internal
        //     and Microsoft.CodeAnalysis.Accessibility.ProtectedAndInternal).
        ProtectedOrInternal = 5,
        //
        // Summary:
        //     Accessible wherever either protected or friend members are accessible (less restrictive
        //     than Microsoft.CodeAnalysis.Accessibility.Protected, Microsoft.CodeAnalysis.Accessibility.Friend
        //     and Microsoft.CodeAnalysis.Accessibility.ProtectedAndFriend).
        ProtectedOrFriend = 5,
        Public = 6
    }
}