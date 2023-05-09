using System.IO;
using System.Text.Json;
using PathFinder.Geom;
using PathFinder.IO;

namespace PathFinderTests.IO
{
    public class ToJsonTests
    {
        private Graph GenSampleGraph()
        {
            var edges = new List<IEdge>()
            {
                new NonDirectionalEdge(0, 1),
                new NonDirectionalEdge(0, 2),
                new NonDirectionalEdge(1, 2)
            };
            var nodes = new List<INode>()
            {
                new CoreNode(0),
                new CoreNode(1),
                new CoreNode(2)
            };
            return new Graph(edges, nodes);
        }

        [Fact]
        public void DumpsTest()
        {
            var graph = GenSampleGraph();
            var actual = ToJson.Dumps(graph, new JsonSerializerOptions() { WriteIndented = false});
            var expected = "{\"EdgeType\":\"NonDirectionalEdge\",\"NodeType\":\"CoreNode\",\"Edges\":[{\"Start\":0,\"End\":1,\"Cost\":1,\"Directed\":false},{\"Start\":0,\"End\":2,\"Cost\":1,\"Directed\":false},{\"Start\":1,\"End\":2,\"Cost\":1,\"Directed\":false}],\"Nodes\":[{\"Index\":0},{\"Index\":1},{\"Index\":2}]}";
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DumpsUninitilizedGraphTest()
        {
            var emptyGraph = new Graph();
            Assert.Throws<ArgumentException>(() => ToJson.Dumps(emptyGraph));
        }

        [Fact]
        public void DumpTest()
        {
            // Generate sample graph
            var graph = GenSampleGraph();

            // Save to json
            var path = "output.json";
            ToJson.Dump(path, graph);

            // Test
            var isExists = File.Exists(path);
            Assert.True(isExists);

            // Remove output file
            File.Delete(path);
        }
    }
}
