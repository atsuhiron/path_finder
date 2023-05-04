using PathFinder.Geom;

namespace PathFinderTests.Geom
{
    public class NodeTests
    {
        [Fact]
        public void CoreNodeGetHashCodeTest()
        {
            var node1 = new CoreNode(1);
            var node2 = new CoreNode(2);
            var node3 = new CoreNode(2);  // Equal to node2

            Assert.NotEqual(node1.GetHashCode(), node2.GetHashCode());
            Assert.Equal(node2.GetHashCode(), node3.GetHashCode());
        }

        [Fact]
        public void XYNodeGetHashCodeTest()
        {
            var node1 = new XYNode(1, 34, 21);
            var node2 = new XYNode(2, 34, 21);
            var node3 = new XYNode(2, 99, 99);
            var node4 = new XYNode(2, 99, 99);  // Equal to node3

            Assert.NotEqual(node1.GetHashCode(), node2.GetHashCode());
            Assert.NotEqual(node1.GetHashCode(), node3.GetHashCode());
            Assert.NotEqual(node2.GetHashCode(), node3.GetHashCode());
            Assert.Equal(node3.GetHashCode(), node4.GetHashCode());
        }
    }
}
