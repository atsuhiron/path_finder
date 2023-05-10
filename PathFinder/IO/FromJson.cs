using System.Text.Json;
using PathFinder.Geom;

namespace PathFinder.IO
{
    public static class FromJson
    {
        public static Graph Loads(string jsonString)
        {
            var dict = ParseJson(jsonString) ?? new Dictionary<string, dynamic?>();

            Func<Dictionary<string, dynamic?>, IEdge> edgeParser = dict["EdgeType"] switch
            {
                "NonDirectionalEdge" => NonDirectionalEdge.Parse,
                _ => throw new NotSupportedException()
            };

            Func<Dictionary<string, dynamic?>, INode> nodeParser = dict["NodeType"] switch
            {
                "CoreNode" => CoreNode.Parse,
                "XYNode" => XYNode.Parse,
                _ => throw new NotSupportedException()
            };

            List<object> edgeObjects = dict["Edges"] as List<object> ?? new List<object>();
            var edges = edgeObjects.Select(obj => edgeParser(obj as Dictionary<string, dynamic?> ?? throw new ArgumentException("不正なJSONです"))).ToList();

            List<object> nodeObjects = dict["Nodes"] as List<object> ?? new List<object>();
            var nodes = nodeObjects.Select(obj => nodeParser(obj as Dictionary<string, dynamic?> ?? throw new ArgumentException("不正なJSONです"))).ToList();

            return new Graph(edges, nodes);
        }

        public static Graph Load(string filePath)
        {
            return Loads(ReadFile(filePath));
        }

        private static Dictionary<string, dynamic?> ParseJson(string json)
        {
            var dic = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(json) ?? new Dictionary<string, JsonElement>();

            return dic.Select(d => new { key = d.Key, value = ParseJsonElement(d.Value) }).ToDictionary(a => a.key, a => a.value);
        }

        private static dynamic? ParseJsonElement(JsonElement elem)
        {
            return elem.ValueKind switch
            {
                JsonValueKind.String => elem.GetString(),
                JsonValueKind.Number => elem.GetInt32(),
                JsonValueKind.False => false,
                JsonValueKind.True => true,
                JsonValueKind.Array => elem.EnumerateArray().Select(e => ParseJsonElement(e)).ToList(),
                JsonValueKind.Null => null,
                JsonValueKind.Object => ParseJson(elem.GetRawText()),
                JsonValueKind.Undefined => throw new NotSupportedException(),
                _ => throw new NotSupportedException(),
            };
        }

        private static string ReadFile(string fileName)
        {
            using FileStream openStream = File.OpenRead(fileName);
            var reader = new StreamReader(openStream);
            return reader.ReadToEnd();
        }
    }
}
