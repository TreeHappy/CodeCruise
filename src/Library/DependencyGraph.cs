using FastGraph;
using FastGraph.Algorithms;

namespace Library
{
    public static class DependencyGraph
    {
        public static (AdjacencyGraph<Vertex, Edge> graph, Dictionary<Vertex, int> components) Build(IEnumerable<Vertex> vertices, IEnumerable<Edge> edges)
        {
            var graph =
                new FastGraph.AdjacencyGraph<Vertex, Edge>();

            graph.AddVertexRange(vertices);

            foreach(var edge in edges)
            {
                try { graph.AddEdge(edge); } catch {}
            }

            var components = new Dictionary<Vertex, int>();

            graph.WeaklyConnectedComponents(components);

            // var xxx = new ClusteredAdjacencyGraph<Vertex, Edge>(graph);
            // var c = xxx.AddCluster();

            return (graph, components);
        }
    }
}
