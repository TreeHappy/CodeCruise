using Library.Structure;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Library
{
    public class ProjectTypeCollector : SymbolVisitor
    {
        private readonly CancellationToken _cancellationToken;
        private readonly SemanticModel semanticModel;
        private readonly Identifier projectIdentifier;

        public Structure.Project Project { get; private set; }

        public ProjectTypeCollector(CancellationToken cancellation, SemanticModel semanticModel, Structure.Identifier projectIdentifier)
        {
            _cancellationToken = cancellation;
            this.semanticModel = semanticModel;
            this.projectIdentifier = projectIdentifier;
        }


        public override void VisitAssembly(IAssemblySymbol symbol)
        {
            _cancellationToken.ThrowIfCancellationRequested();

            var assemblyIdentifier =
                new Structure.Identifier(symbol.Name);
            var assembly =
                new Structure.Assembly
                    ( assemblyIdentifier
                    , new Structure.Reference<Structure.Project>(projectIdentifier, typeof(Structure.Project))
                    , new Dictionary<Structure.Identifier, Structure.Namespace>()
                    , new Dictionary<Structure.Identifier, Structure.Assembly>()
                    , new Dictionary<Structure.Identifier, Structure.Attribute>()
                    );

            Project =
                new Structure.Project
                    ( projectIdentifier
                    , assembly
                    );

            assembly.Parent.SetIdentifier(Project);

            symbol.GlobalNamespace.Accept(this);
        }

        public override void VisitNamespace(INamespaceSymbol symbol)
        {
            if (symbol.IsGlobalNamespace is false)
            {
                var namespaceIdentifier = new Structure.Identifier(symbol.ToString());
                IEitherNamespaceOrAssembly parent =
                    symbol.ContainingNamespace switch
                        { INamespaceSymbol ns when ns.IsGlobalNamespace =>
                            Project.Assembly
                        , INamespaceSymbol ns =>
                            Project.Assembly.Namespaces.Single(n => n.Key.Name == ns.ToString()).Value
                        , null =>
                            Project.Assembly
                        };

                Project
                    .Assembly
                    .Namespaces
                    .TryAdd
                        ( namespaceIdentifier
                        , new Structure.Namespace
                            ( namespaceIdentifier
                            , new Reference<IEitherNamespaceOrAssembly>(parent)
                            , new Dictionary<Structure.Identifier, Structure.Namespace>()
                            , new Dictionary<Structure.Identifier, Structure.Type>()
                            )
                        );
            }

            foreach (INamespaceOrTypeSymbol namespaceOrType in symbol.GetMembers())
            {
                _cancellationToken.ThrowIfCancellationRequested();

                namespaceOrType.Accept(this);
            }
        }

        public override void VisitMethod(IMethodSymbol symbol)
        {
            // var syntaxVisitor = new SyntaxVisitor(semanticModel);

            // iterate method body
            // var identifiers =
            //     symbol
            //         .DeclaringSyntaxReferences
            //         .SelectMany(
            //             d =>
            //                 d
            //                 .GetSyntax()
            //                 .DescendantNodes()
            //                 .OfType<CSharpSyntaxNode>()
            //         );

            // if (identifiers is not null) foreach (var a in identifiers)
            // {
            //     a.Accept(syntaxVisitor);
            // }

            // iterate public interface types
            // foreach (var s in symbol.Parameters)
            //     try { Console.WriteLine(s.Type.ContainingNamespace.Name); } catch {}
        }

        public override void VisitNamedType(INamedTypeSymbol type)
        {
            _cancellationToken.ThrowIfCancellationRequested();

            if (type.ContainingNamespace.IsGlobalNamespace)
                return;

            var namespaceIdentifer = new Structure.Identifier(type.ContainingNamespace.ToString());
            var typeIdentifier = new Structure.Identifier(type.Name);
            var @namespace = Project.Assembly.Namespaces[namespaceIdentifer];

            @namespace.Types
                .TryAdd
                    ( typeIdentifier
                    , new Structure.Type
                        ( typeIdentifier
                        , new Reference<IEitherTypeOrNamespace>(@namespace)
                        , Structure.Conversion.From(type.DeclaredAccessibility)
                        , new Dictionary<Structure.Identifier, Structure.Method>()
                        , new Dictionary<Structure.Identifier, Structure.Property>()
                        , new Dictionary<Structure.Identifier, Structure.Field>()
                        , new Dictionary<Structure.Identifier, Reference<Structure.Attribute>>()
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