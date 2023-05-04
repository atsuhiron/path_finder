using PathFinder.Geom;

namespace PathFinderTests.Geom
{
    public class EdgeTest
    {
        [Fact]
        public void NDEdgeGetHashCodeTest()
        {
            var edge1 = new NonDirectionalEdge(0, 1, 1);
            var edge2 = new NonDirectionalEdge(0, 1, 1);  // Equal to edge1
            var edge3 = new NonDirectionalEdge(0, 1, 4);  // Same path with edge1 but it has different cost, so NOT equal to edge1
         
            Assert.Equal(edge1.GetHashCode(), edge2.GetHashCode());
            Assert.NotEqual(edge1.GetHashCode(), edge3.GetHashCode());
        }
    }
}
