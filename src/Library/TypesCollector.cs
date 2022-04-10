using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Library
{
    public class TypeCollector : SymbolVisitor
    {
        private readonly CancellationToken _cancellationToken;
        private readonly SemanticModel semanticModel;
        private Strcuture.Project currentProject;

        public TypeCollector(CancellationToken cancellation, SemanticModel semanticModel)
        {
            _cancellationToken = cancellation;
            this.semanticModel = semanticModel;
        }


        public override void VisitAssembly(IAssemblySymbol symbol)
        {
            _cancellationToken.ThrowIfCancellationRequested();

            var assemblyIdentifier = new Strcuture.Identifier(symbol.Name);
            var assembly =
                new Strcuture.Assembly
                    ( assemblyIdentifier
                    , new Dictionary<Strcuture.Identifier, Strcuture.Namespace>()
                    , new Dictionary<Strcuture.Identifier, Strcuture.Assembly>()
                    , new Dictionary<Strcuture.Identifier, Attribute>()
                    );

            currentProject =
                new Strcuture.Project
                    ( new Strcuture.Identifier("")
                    , assembly
                    );

            symbol.GlobalNamespace.Accept(this);
        }

        public override void VisitNamespace(INamespaceSymbol symbol)
        {
            foreach (INamespaceOrTypeSymbol namespaceOrType in symbol.GetMembers())
            {
                _cancellationToken.ThrowIfCancellationRequested();

                var namespaceIdentifier = new Strcuture.Identifier(symbol.Name);

                currentProject
                    .Assembly
                    .Namespaces
                    .TryAdd
                        ( namespaceIdentifier
                        , new Strcuture.Namespace(namespaceIdentifier, new Dictionary<Strcuture.Identifier, Strcuture.Namespace>()
                        , new Dictionary<Strcuture.Identifier, Strcuture.Type>())
                        );

                namespaceOrType.Accept(this);
            }
        }

        public override void VisitMethod(IMethodSymbol symbol)
        {
            Console.WriteLine(symbol.ToString());

            var syntaxVisitor = new SyntaxVisitor(semanticModel);

            var identifiers =
                symbol
                    .DeclaringSyntaxReferences
                    .SelectMany(
                        d =>
                            d
                            .GetSyntax()
                            .DescendantNodes()
                            .OfType<CSharpSyntaxNode>()
                    );

            if (identifiers is not null) foreach (var a in identifiers)
            {
                a.Accept(syntaxVisitor);
            }

            // foreach (var s in symbol.Parameters)
            //     try { Console.WriteLine(s.Type.ContainingNamespace.Name); } catch {}

            // Console.WriteLine(symbol.DeclaringSyntaxReferences.First().GetSyntax().GetText());
        }

        public override void VisitNamedType(INamedTypeSymbol type)
        {
            _cancellationToken.ThrowIfCancellationRequested();

            var namespaceIdentifer = new Strcuture.Identifier(type.ContainingNamespace.Name);
            var typeIdentifier = new Strcuture.Identifier(type.Name);

            currentProject.Assembly.Namespaces[namespaceIdentifer].Types
                .TryAdd
                    ( typeIdentifier
                    , new Strcuture.Type
                        ( typeIdentifier
                        , Strcuture.Conversion.From(type.DeclaredAccessibility)
                        , new Dictionary<Strcuture.Identifier, Strcuture.Method>()
                        , new Dictionary<Strcuture.Identifier, Strcuture.Property>()
                        , new Dictionary<Strcuture.Identifier, Strcuture.Field>()
                        , new Dictionary<Strcuture.Identifier, Strcuture.Attibute>()
                        )
                    );

            var nestedTypes = type.GetTypeMembers();
            var members = type.GetMembers();

            foreach (var symbol in members)
            {
                _cancellationToken.ThrowIfCancellationRequested();
                symbol.Accept(this);
            }

            foreach (INamedTypeSymbol nestedType in nestedTypes)
            {
                _cancellationToken.ThrowIfCancellationRequested();
                nestedType.Accept(this);
            }
        }
    }
}