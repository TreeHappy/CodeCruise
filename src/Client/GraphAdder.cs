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
            throw new NotImplementedException();
        }

        protected override void OnProject(Project project)
        {
            throw new NotImplementedException();
        }

        protected override void OnType(Library.Structure.Type type)
        {
            throw new NotImplementedException();
        }
    }
}