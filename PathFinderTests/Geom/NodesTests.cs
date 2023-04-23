using PathFinder.Geom;

namespace PathFinderTests.Geom
{
    public class NodesTests
    {
        [Fact]
        public void ConstructoeTestWithNoArg()
        {
            var nodes = new Nodes();
            Assert.Empty(nodes.NodeIndices);
            Assert.Empty(nodes.Edges);
        }

        [Fact]
        public void ConstructorTestWithEdges()
        {
            var edges = new List<IEdge>() { new NonDirectionalEdge(1, 0) };
            var nodes = new Nodes(edges);

            Assert.Equal(2, nodes.NodeIndices.Count);
            Assert.Equal(0, nodes.NodeIndices[0]);
            Assert.Equal(1, nodes.NodeIndices[1]);
        }

        [Fact]
        public void ConstructorTestWithDuplicatedEdges()
        {
            var edges = new List<IEdge>() { new NonDirectionalEdge(1, 0), new NonDirectionalEdge(0, 1) };
            var nodes = new Nodes(edges);

            Assert.Single(nodes.Edges);
            Assert.Equal(2, nodes.NodeIndices.Count);
            Assert.Equal(0, nodes.NodeIndices[0]);
            Assert.Equal(1, nodes.NodeIndices[1]);
        }

        [Fact]
        public void ConstructoeTestWitEdgesAndNodes()
        {
            var edges = new List<IEdge>() { new NonDirectionalEdge(1, 0) };
            var nodeIndices = new List<int>() { 1, 0 };
            var nodes = new Nodes(edges, nodeIndices);

            Assert.Equal(0, nodes.NodeIndices[0]);
            Assert.Equal(1, nodes.NodeIndices[1]);
        }

        [Fact]
        public void ConstructoeTestWitEdgesAndNodesRaiseException()
        {
            var edges = new List<IEdge>() { new NonDirectionalEdge(1, 0), new NonDirectionalEdge(0, 2) };
            var nodeIndices = new List<int>() { 1, 0 };

            _ = Assert.Throws<ArgumentException>(() => new Nodes(edges, nodeIndices));
        }

        [Fact]
        public void AddEdgeNormalTest()
        {
            var sut = new Nodes(new List<IEdge>() { new NonDirectionalEdge(1, 0), new NonDirectionalEdge(0, 2) });

            Assert.Equal(2, sut.Edges.Count);
            sut.AddEdge(new NonDirectionalEdge(1, 2), false);
            Assert.Equal(3, sut.Edges.Count);
        }

        [Fact]
        public void AddEdgeDuplicatedTest()
        {
            var sut = new Nodes(new List<IEdge>() { new NonDirectionalEdge(1, 0), new NonDirectionalEdge(0, 2) });

            Assert.Equal(2, sut.Edges.Count);
            sut.AddEdge(new NonDirectionalEdge(0, 1), false);
            Assert.Equal(2, sut.Edges.Count);
        }

        [Fact]
        public void AddEdgeRaiseExceptionTest()
        {
            var sut = new Nodes(new List<IEdge>() { new NonDirectionalEdge(1, 0), new NonDirectionalEdge(0, 2) });

            _ = Assert.Throws<ArgumentException>(() => sut.AddEdge(new NonDirectionalEdge(0, 3), false));
        }

        [Fact]
        public void AddEdgeAndAddNodeTest()
        {
            var sut = new Nodes(new List<IEdge>() { new NonDirectionalEdge(1, 0), new NonDirectionalEdge(0, 3) });

            Assert.Equal(2, sut.Edges.Count);
            Assert.Equal(3, sut.NodeIndices.Count);
            Assert.Equal(3, sut.NodeIndices[2]);
            sut.AddEdge(new NonDirectionalEdge(0, 2), true);
            Assert.Equal(3, sut.Edges.Count);
            Assert.Equal(4, sut.NodeIndices.Count);
            Assert.Equal(2, sut.NodeIndices[2]);
        }

        [Fact]
        public void CreateGridTest()
        {
            // 0 - 1 - 2
            // |   |   |
            // 3 - 4 - 5
            var sut = Nodes.CreateGrid(3, 2);

            Assert.Contains(0, sut.NodeIndices);
            Assert.Contains(1, sut.NodeIndices);
            Assert.Contains(2, sut.NodeIndices);
            Assert.Contains(3, sut.NodeIndices);
            Assert.Contains(4, sut.NodeIndices);
            Assert.Contains(5, sut.NodeIndices);

            Assert.Contains(new NonDirectionalEdge(0, 1), sut.Edges);
            Assert.Contains(new NonDirectionalEdge(1, 2), sut.Edges);
            Assert.Contains(new NonDirectionalEdge(0, 3), sut.Edges);
            Assert.Contains(new NonDirectionalEdge(1, 4), sut.Edges);
            Assert.Contains(new NonDirectionalEdge(2, 5), sut.Edges);
            Assert.Contains(new NonDirectionalEdge(3, 4), sut.Edges);
            Assert.Contains(new NonDirectionalEdge(4, 5), sut.Edges);
        }

        [Fact]
        public void GetAdjacenciesTest()
        {
            // 0 - 1 - 2
            // |   |   |
            // 3 - 4 - 5
            var sut = Nodes.CreateGrid(3, 2);
            var adj = sut.GetAdjacencies(1);
            Assert.Equal(new List<int>() { 0, 2, 4 }, adj);
        }

        [Fact]
        public void GetEdgeNormalTest()
        {
            // 0 - 1 - 2
            // |   |   |
            // 3 - 4 - 5
            var sut = Nodes.CreateGrid(3, 2);
            var edgeOrdinal = sut.GetEdge(0, 3);
            var edgeInverse = sut.GetEdge(3, 0);
            Assert.True(edgeOrdinal.Equals(edgeInverse));
        }

        [Fact]
        public void GetEdgeNotFoundTest()
        {
            // 0 - 1 - 2
            // |   |   |
            // 3 - 4 - 5
            var sut = Nodes.CreateGrid(3, 2);

            _ = Assert.Throws<ArgumentException>(() => sut.GetEdge(0, 4));
            _ = Assert.Throws<ArgumentException>(() => sut.GetEdge(4, 0));
        }

        [Fact]
        public void GetEdgeLowestCostTest()
        {
            //   3.2
            // 0 === 1
            //   1.5
            var sut = new Nodes(new List<IEdge>()
            {
                new NonDirectionalEdge(0, 1, 3.2f),
                new NonDirectionalEdge(0, 1, 1.5f)
            });

            var lowestEdge = sut.GetEdge(0, 1);
            Assert.Equal(1.5, lowestEdge.Cost);
        }
    }
}
