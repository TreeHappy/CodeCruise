using Library;
using Library.Structure;

namespace Client
{
    public class GraphAdder : Iterator
    {
        private readonly List<Vertex> vertices;
        private readonly List<Edge> edges;

        public GraphAdder(List<Vertex> vertices, List<Edge> edges)
        {
            this.vertices = vertices;
            this.edges = edges;
        }

        protected override void OnNamespace(Namespace @namespace)
        {
            if (vertices.Any(v => v.Name == @namespace.Identifier.Name))
                return;

            var namespaceVertex =
                new Vertex(@namespace.Identifier.Name, VertexKind.Namespace);
            var parentVertex =
                new Vertex
                    ( @namespace.Parent.Identifier.Name
                    , @namespace.Parent.TypeName switch
                        { "Library.Structure.Assembly" => VertexKind.Project
                        , "Library.Structure.Namespace" => VertexKind.Namespace
                        , string n => throw new Exception(n)
                        }
                    );

            vertices.Add(namespaceVertex);
            edges.Add(new Edge(parentVertex, namespaceVertex));
        }

        protected override void OnProject(Project project)
        {
        }

        protected override void OnType(Library.Structure.Type type)
        {
        }
    }
}