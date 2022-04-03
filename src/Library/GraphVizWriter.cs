using FastGraph;
using FastGraph.Graphviz;

namespace Library
{
    public static class GraphVizWriter
    {
        public static void WriteDot((AdjacencyGraph<Vertex, Edge> graph, Dictionary<Vertex, int> components) dependencyGraph, string fileName)
        {
            var graphviz =
                dependencyGraph.graph
                .ToGraphviz
                    ( algorithm =>
                        algorithm.FormatVertex += (_, args) =>
                            {
                                args.VertexFormat.Label = args.Vertex.Name;
                            }
                    );


            File.WriteAllText(fileName, graphviz);
        }
    }
}