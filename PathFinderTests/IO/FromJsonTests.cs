using PathFinder.Geom;
using PathFinder.IO;

namespace PathFinderTests.IO
{
    public class FromJsonTests
    {
        [Fact]
        public void LoadsTest()
        {
            var graphJsonString = "{\"EdgeType\":\"NonDirectionalEdge\",\"NodeType\":\"CoreNode\",\"Edges\":[{\"Start\":0,\"End\":1,\"Cost\":1,\"Directed\":false},{\"Start\":0,\"End\":2,\"Cost\":1,\"Directed\":false},{\"Start\":1,\"End\":2,\"Cost\":1,\"Directed\":false}],\"Nodes\":[{\"Index\":0},{\"Index\":1},{\"Index\":2}]}";
            var graph = FromJson.Loads(graphJsonString);
            Assert.Equal(3, graph.Edges.Count);
            Assert.Equal(3, graph.Nodes.Count);
        }

        [Fact]
        public void LoadTest()
        {
            var fileName = "../../../../PathFinder/samples/graph_sample.json";
            var graph = FromJson.Load(fileName);
            Assert.Equal(3, graph.Edges.Count);
            Assert.Equal(3, graph.Nodes.Count);
        }
    }
}
