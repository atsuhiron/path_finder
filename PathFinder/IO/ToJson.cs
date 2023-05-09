using System.Text.Json;
using PathFinder.Geom;

namespace PathFinder.IO
{
    public static class ToJson
    {
        public static string Dumps(Graph graph, JsonSerializerOptions? options = default)
        {
            CheckGraph(graph);
            if (options == default) options = CreateDefaultOptions();

            return JsonSerializer.Serialize(graph, options);
        }

        public async static void Dump(string fileName, Graph graph, JsonSerializerOptions? options = default)
        {
            CheckGraph(graph);
            if (options == default) options = CreateDefaultOptions();

            using FileStream createStream = File.Create(fileName);
            await JsonSerializer.SerializeAsync(createStream, graph, options);
            await createStream.DisposeAsync();
        }

        private static JsonSerializerOptions CreateDefaultOptions() => new() { WriteIndented = true };

        private static void CheckGraph (in Graph graph)
        {
            if (graph == null) throw new ArgumentException("Graph が null です。");
            if (string.IsNullOrEmpty(graph.NodeType)) throw new ArgumentException("NodeType が未定義です。");
            if (string.IsNullOrEmpty(graph.EdgeType)) throw new ArgumentException("EdgeType が未定義です。");
        } 
    }
}
