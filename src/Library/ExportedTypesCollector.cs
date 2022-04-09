using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

namespace Library
{
    public class ExportedTypesCollector : SymbolVisitor
    {
        public static bool IsAccessibleOutsideOfAssembly(ISymbol symbol) =>
                symbol.DeclaredAccessibility switch
                {
                    Accessibility.Private => false,
                    Accessibility.Internal => false,
                    Accessibility.ProtectedAndInternal => false,
                    Accessibility.Protected => true,
                    Accessibility.ProtectedOrInternal => true,
                    Accessibility.Public => true,
                    _ => true,    //Here should be some reasonable default
                };

        private readonly CancellationToken _cancellationToken;
        private readonly HashSet<INamedTypeSymbol> _exportedTypes;

        public ExportedTypesCollector(CancellationToken cancellation, int? estimatedCapacity = null)
        {
            _cancellationToken = cancellation;
            _exportedTypes = estimatedCapacity.HasValue
                ? new HashSet<INamedTypeSymbol>(estimatedCapacity.Value, SymbolEqualityComparer.Default)
                : new HashSet<INamedTypeSymbol>(SymbolEqualityComparer.Default);
        }

        public ImmutableArray<INamedTypeSymbol> GetPublicTypes() => _exportedTypes.ToImmutableArray();

        public override void VisitAssembly(IAssemblySymbol symbol)
        {
            _cancellationToken.ThrowIfCancellationRequested();
            symbol.GlobalNamespace.Accept(this);
        }

        public override void VisitNamespace(INamespaceSymbol symbol)
        {
            foreach (INamespaceOrTypeSymbol namespaceOrType in symbol.GetMembers())
            {
                _cancellationToken.ThrowIfCancellationRequested();
                namespaceOrType.Accept(this);
            }
        }

        public override void VisitNamedType(INamedTypeSymbol type)
        {
            _cancellationToken.ThrowIfCancellationRequested();

            Console.WriteLine(type.ToString());

            if (!IsAccessibleOutsideOfAssembly(type) || !_exportedTypes.Add(type))
                return;

            var nestedTypes = type.GetTypeMembers();
            var members = type.GetMembers();

            foreach (var symbol in members)
            {
                _cancellationToken.ThrowIfCancellationRequested();
                symbol.Accept(this);
            }

            // if (nestedTypes.IsDefaultOrEmpty)
            //     return;

            foreach (INamedTypeSymbol nestedType in nestedTypes)
            {
                _cancellationToken.ThrowIfCancellationRequested();
                nestedType.Accept(this);
            }
        }
    }
}