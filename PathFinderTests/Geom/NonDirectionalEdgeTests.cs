using PathFinder.Geom;

namespace PathFinderTests.Geom
{
    public class NonDirectionalEdgeTests
    {
        [Fact]
        public void NotEqualTest()
        {
            var edge = new NonDirectionalEdge(0, 1);
            var otherEdge = new NonDirectionalEdge(0, 2);

            Assert.False(edge.Equals(otherEdge));
        }

        [Fact]
        public void EqualTest()
        {
            var edge = new NonDirectionalEdge(0, 1);
            var inverseEdge = new NonDirectionalEdge(1, 0);

            Assert.True(edge.Equals(inverseEdge));
        }

        [Fact]
        public void EdgeHashTest()
        {
            var edge1 = new NonDirectionalEdge(0, 1);
            var edge2 = new NonDirectionalEdge(0, 2);
            var edge3 = new NonDirectionalEdge(0, 1);
            var edge4 = new NonDirectionalEdge(1, 0);

            var edgeHash = new HashSet<IEdge>(new List<IEdge>() { edge1, edge2, edge3, edge4 });
            Assert.Equal(2, edgeHash.Count);
        }

        [Fact]
        public void ContainingInListTest()
        {
            var edges = new List<IEdge>() { new NonDirectionalEdge(0, 1) };
            var target1 = new NonDirectionalEdge(0, 1);
            var target2 = new NonDirectionalEdge(1, 0);

            Assert.Contains(target1, edges);
            Assert.Contains(target2, edges);
        }
    }
}
