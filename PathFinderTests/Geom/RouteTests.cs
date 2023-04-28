using PathFinder.Geom;

namespace PathFinderTests.Geom
{
    public class RouteTests
    {
        [Fact]
        public void GenerateNodeListTest()
        {
            var edges = new List<IEdge>()
            {
                new NonDirectionalEdge(0, 1),
                new NonDirectionalEdge(1, 4),
                new NonDirectionalEdge(4, 2),
                new NonDirectionalEdge(2, 5)
            };
            var expectedNodeIndices = new List<int>() { 0, 1, 4, 2, 5 };

            var sut = new Route(edges, 0);
            Assert.Equal(expectedNodeIndices, sut.RouteNodeIndices);
        }

        [Fact]
        public void DiscontinuousEdgeTest()
        {
            var edges = new List<IEdge>()
            {
                new NonDirectionalEdge(0, 1),
                new NonDirectionalEdge(1, 4),
                new NonDirectionalEdge(4, 3),  // (4, 2) is correct
                new NonDirectionalEdge(2, 5)
            };

            _ = Assert.Throws<ArgumentException>(() => new Route(edges, 0));
        }

        [Fact]
        public void SumCostTest()
        {
            var edges = new List<IEdge>()
            {
                new NonDirectionalEdge(0, 1),
                new NonDirectionalEdge(1, 4, 3),
                new NonDirectionalEdge(4, 2, 5),
                new NonDirectionalEdge(2, 5, 2)
            };

            var sut = new Route(edges, 0);
            Assert.Equal(1 + 3 + 5 + 2, sut.SumCost());
        }
    }
}
