using PathFinder.Geom;
using PathFinder.PathFinderAlgorithm;

namespace PathFinderTests.PathFinderAlgorithm
{
    public class AStarBDTests
    {
        [Fact]
        public void FindRouteNoEdgeTest()
        {
            //   3.2
            // 0 --- 1
            var _graph = new Graph(
                new List<IEdge>() { new NonDirectionalEdge(0, 1, 3.2f) },
                new List<INode> { new XYNode(0, 0, 0), new XYNode(1, 1, 0) }
            );

            var astar = new AStarBD<XYNode, NonDirectionalEdge>(_graph);
            var route = astar.FindRoute(0, 0);

            Assert.True(route.Success);
            Assert.Empty(route.RouteEdges);
            Assert.Single(route.RouteNodeIndices);

            Assert.Equal(0, route.RouteNodeIndices[0]);
        }

        [Fact]
        public void FindRouteOneEdgeTest()
        {
            //   3.2
            // 0 --- 1
            var _graph = new Graph(
                new List<IEdge>()
                {
                    new NonDirectionalEdge(0, 1, 3.2f)
                },
                new List<INode>
                {
                    new XYNode(0, 0, 0),
                    new XYNode(1, 1, 0)
                }
            );

            var astar = new AStarBD<XYNode, NonDirectionalEdge>(_graph);
            var route = astar.FindRoute(0, 1);

            Assert.True(route.Success);
            Assert.Single(route.RouteEdges);
            Assert.Equal(2, route.RouteNodeIndices.Count);

            Assert.Equal(0, route.RouteNodeIndices[0]);
            Assert.Equal(1, route.RouteNodeIndices[1]);
        }

        [Fact]
        public void FindRouteSinglePathTest()
        {
            //   3.2   1.0
            // 0 --- 1 --- 2
            var _graph = new Graph(
                new List<IEdge>()
                {
                    new NonDirectionalEdge(0, 1, 3.2f),
                    new NonDirectionalEdge(1, 2, 1.0f)
                },
                new List<INode>
                {
                    new XYNode(0, 0, 0),
                    new XYNode(1, 1, 0),
                    new XYNode(2, 2, 0)
                }
            );

            var astar = new AStarBD<XYNode, NonDirectionalEdge>(_graph);
            var route = astar.FindRoute(0, 2);

            Assert.True(route.Success);
            Assert.Equal(3, route.RouteNodeIndices.Count);

            Assert.Equal(0, route.RouteNodeIndices[0]);
            Assert.Equal(1, route.RouteNodeIndices[1]);
            Assert.Equal(2, route.RouteNodeIndices[2]);
        }

        [Fact]
        public void FindRouteGridTest()
        {
            var _graph = Graph.CreateXYGrid(10, 10);

            var astar = new AStarBD<XYNode, NonDirectionalEdge>(_graph);
            var route = astar.FindRoute(0, 99);

            Assert.True(route.Success);
            Assert.True(route.Iteration < 99);
        }
    }
}
