using PathFinder.Geom;
using PathFinder.PathFinderAlgorithm;

namespace PathFinderTests.PathFinderAlgorithm
{
    public class DijkstraTests
    {
        [Fact]
        public void FindRouteSinglePathTest()
        {
            //   3.2   1.0
            // 0 --- 1 --- 2
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

        [Fact]
        public void FindRouteMultiPathTest()
        {
            //          0.5   1.5
            //        1 --- 2 --- 3
            // 1.5 -> |    12.25  | <- 0.25
            //        0 --------- 4
            // 3.5 -> |           | <- 3.5
            //        5 ----------6
            //              1.0
            //
            // 0 -> 1 -> 2 -> 3 -> 4 :  3.75
            // 0 -> 4                : 12.25
            // 0 -> 5 -> 6 -> 4      :  8.0

            var _nodes = new Nodes(new List<IEdge>()
            {
                new NonDirectionalEdge(0, 1, 1.5f),
                new NonDirectionalEdge(1, 2, 0.5f),
                new NonDirectionalEdge(2, 3, 1.5f),
                new NonDirectionalEdge(3, 4, 0.25f),
                new NonDirectionalEdge(0, 4, 12.25f),
                new NonDirectionalEdge(0, 5, 3.5f),
                new NonDirectionalEdge(5, 6, 1.0f),
                new NonDirectionalEdge(6, 4, 3.5f),
            });

            var dijkstra = new Dijkstra(_nodes);
            var route = dijkstra.FindRoute(0, 4);

            Assert.True(route.Success);
            Assert.Equal(5, route.RouteNodeIndices.Count);

            Assert.Equal(0, route.RouteNodeIndices[0]);
            Assert.Equal(1, route.RouteNodeIndices[1]);
            Assert.Equal(2, route.RouteNodeIndices[2]);
            Assert.Equal(3, route.RouteNodeIndices[3]);
            Assert.Equal(4, route.RouteNodeIndices[4]);
        }
    }
}
