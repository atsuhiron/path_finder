using PathFinder.Geom;
using PathFinder.PathFinderAlgorithm;

namespace PathFinderTests.PathFinderAlgorithm
{
    public class DijkstraTests
    {
        [Fact]
        public void FindRouteTest()
        {
            // 0 - 1 - 2
            var _nodes = new Nodes(new List<IEdge>()
            {
                new NonDirectionalEdge(0, 1, 3.2f),
                new NonDirectionalEdge(1, 2, 1.0f)
            });

            var dijkstra = new Dijkstra(_nodes);
            var route = dijkstra.FindRoute(0, 2);

            Assert.True(route.Success);
            Assert.Equal(3, route.RouteNodeIndices.Count);
            Assert.Equal(0, route.RouteNodeIndices[0]);
            Assert.Equal(1, route.RouteNodeIndices[1]);
            Assert.Equal(2, route.RouteNodeIndices[2]);
        }
    }
}
