using PathFinder.Geom;

namespace PathFinderTests.Geom
{
    public class GraphTests
    {
        [Fact]
        public void ConstructoeTestWithNoArg()
        {
            var graph = new Graph();
            Assert.Empty(graph.Nodes);
            Assert.Empty(graph.Edges);
        }

        [Fact]
        public void ConstructorTestWithEdges()
        {
            var edges = new List<IEdge>() { new NonDirectionalEdge(1, 0) };
            var graph = new Graph(edges, (int index) => new CoreNode(index));

            Assert.Equal(2, graph.Nodes.Count);
            Assert.Equal(0, graph.Nodes[0].Index);
            Assert.Equal(1, graph.Nodes[1].Index);
        }

        [Fact]
        public void ConstructorTestWithDuplicatedEdges()
        {
            var edges = new List<IEdge>() { new NonDirectionalEdge(1, 0), new NonDirectionalEdge(0, 1) };
            var graph = new Graph(edges, (int index) => new CoreNode(index));

            Assert.Single(graph.Edges);
            Assert.Equal(2, graph.Nodes.Count);
            Assert.Equal(0, graph.Nodes[0].Index);
            Assert.Equal(1, graph.Nodes[1].Index);
        }

        [Fact]
        public void ConstructoeTestWitEdgesAndNodes()
        {
            var edges = new List<IEdge>() { new NonDirectionalEdge(1, 0) };
            List<INode> nodes = new List<int>() { 1, 0 }.Select(i => (INode)new CoreNode(i)).ToList();
            var graph = new Graph(edges, nodes);

            Assert.Equal(0, graph.Nodes[0].Index);
            Assert.Equal(1, graph.Nodes[1].Index);
        }

        [Fact]
        public void ConstructoeTestWitEdgesAndNodesRaiseException()
        {
            var edges = new List<IEdge>() { new NonDirectionalEdge(1, 0), new NonDirectionalEdge(0, 2) };
            var nodes = new List<int>() { 1, 0 }.Select(i => (INode)new CoreNode(i)).ToList();

            _ = Assert.Throws<ArgumentException>(() => new Graph(edges, nodes));
        }

        [Fact]
        public void AddEdgeNormalTest()
        {
            var sut = new Graph(
                new List<IEdge>() { new NonDirectionalEdge(1, 0), new NonDirectionalEdge(0, 2) },
                (int index) => new CoreNode(index)
            );

            Assert.Equal(2, sut.Edges.Count);
            sut.AddEdge(new NonDirectionalEdge(1, 2));
            Assert.Equal(3, sut.Edges.Count);
        }

        [Fact]
        public void AddEdgeDuplicatedTest()
        {
            var sut = new Graph(
                new List<IEdge>() { new NonDirectionalEdge(1, 0), new NonDirectionalEdge(0, 2) },
                (int index) => new CoreNode(index)
            );

            Assert.Equal(2, sut.Edges.Count);
            sut.AddEdge(new NonDirectionalEdge(0, 1));
            Assert.Equal(2, sut.Edges.Count);
        }

        [Fact]
        public void AddEdgeRaiseExceptionTest()
        {
            var sut = new Graph(
                new List<IEdge>() { new NonDirectionalEdge(1, 0), new NonDirectionalEdge(0, 2) },
                (int index) => new CoreNode(index)
            );

            _ = Assert.Throws<ArgumentException>(() => sut.AddEdge(new NonDirectionalEdge(0, 3)));
        }

        [Fact]
        public void AddEdgeAndAddNodeTest()
        {
            var sut = new Graph(
                new List<IEdge>() { new NonDirectionalEdge(1, 0), new NonDirectionalEdge(0, 3) },
                (int index) => new CoreNode(index)
            );

            Assert.Equal(2, sut.Edges.Count);
            Assert.Equal(3, sut.Nodes.Count);
            Assert.Equal(3, sut.Nodes[2].Index);
            sut.AddEdge(new NonDirectionalEdge(0, 2), (int index) => new CoreNode(index));
            Assert.Equal(3, sut.Edges.Count);
            Assert.Equal(4, sut.Nodes.Count);
            Assert.Equal(2, sut.Nodes[2].Index);
        }

        [Fact]
        public void CreateGridTest()
        {
            // 0 - 1 - 2
            // |   |   |
            // 3 - 4 - 5
            var sut = Graph.CreateGrid(3, 2);

            var nodeIndices = sut.Nodes.Select(n => n.Index).ToList();
            Assert.Contains(0, nodeIndices);
            Assert.Contains(1, nodeIndices);
            Assert.Contains(2, nodeIndices);
            Assert.Contains(3, nodeIndices);
            Assert.Contains(4, nodeIndices);
            Assert.Contains(5, nodeIndices);

            Assert.Contains(new NonDirectionalEdge(0, 1), sut.Edges);
            Assert.Contains(new NonDirectionalEdge(1, 2), sut.Edges);
            Assert.Contains(new NonDirectionalEdge(0, 3), sut.Edges);
            Assert.Contains(new NonDirectionalEdge(1, 4), sut.Edges);
            Assert.Contains(new NonDirectionalEdge(2, 5), sut.Edges);
            Assert.Contains(new NonDirectionalEdge(3, 4), sut.Edges);
            Assert.Contains(new NonDirectionalEdge(4, 5), sut.Edges);
        }

        [Fact]
        public void CreateXYGridTest()
        {
            // 0 - 1 - 2
            // |   |   |
            // 3 - 4 - 5
            var sut = Graph.CreateXYGrid(3, 2);

            var nodeIndices = sut.Nodes.Select(n => n.Index).ToList();
            Assert.Contains(0, nodeIndices);
            Assert.Contains(1, nodeIndices);
            Assert.Contains(2, nodeIndices);
            Assert.Contains(3, nodeIndices);
            Assert.Contains(4, nodeIndices);
            Assert.Contains(5, nodeIndices);

            var castedNodes = sut.Nodes.Select(n => (XYNode)n).ToList();
            Assert.Equal(0, castedNodes[0].X);
            Assert.Equal(1, castedNodes[1].X);
            Assert.Equal(2, castedNodes[2].X);
            Assert.Equal(0, castedNodes[3].X);
            Assert.Equal(1, castedNodes[4].X);
            Assert.Equal(2, castedNodes[5].X);
            Assert.Equal(0, castedNodes[0].Y);
            Assert.Equal(0, castedNodes[1].Y);
            Assert.Equal(0, castedNodes[2].Y);
            Assert.Equal(1, castedNodes[3].Y);
            Assert.Equal(1, castedNodes[4].Y);
            Assert.Equal(1, castedNodes[5].Y);

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
            var sut = Graph.CreateGrid(3, 2);
            var adj = sut.GetAdjacencies(1);
            Assert.Equal(new List<int>() { 0, 2, 4 }, adj);
        }

        [Fact]
        public void SearchEdgeNormalTest()
        {
            // 0 - 1 - 2
            // |   |   |
            // 3 - 4 - 5
            var sut = Graph.CreateGrid(3, 2);
            var edgeOrdinal = sut.SearchEdge(0, 3);
            var edgeInverse = sut.SearchEdge(3, 0);
            Assert.True(edgeOrdinal.Equals(edgeInverse));
        }

        [Fact]
        public void SearchEdgeNotFoundTest()
        {
            // 0 - 1 - 2
            // |   |   |
            // 3 - 4 - 5
            var sut = Graph.CreateGrid(3, 2);

            _ = Assert.Throws<ArgumentException>(() => sut.SearchEdge(0, 4));
            _ = Assert.Throws<ArgumentException>(() => sut.SearchEdge(4, 0));
        }

        [Fact]
        public void SearchEdgeLowestCostTest()
        {
            //   3.2
            // 0 === 1
            //   1.5
            var sut = new Graph(new List<IEdge>()
            {
                new NonDirectionalEdge(0, 1, 3.2f),
                new NonDirectionalEdge(0, 1, 1.5f)
            },
            (int index) => new CoreNode(index));

            var lowestEdge = sut.SearchEdge(0, 1);
            Assert.Equal(1.5, lowestEdge.Cost);
        }

        [Fact]
        public void SetEdgeTypeTest()
        {
            // Manual setting
            var sut = new Graph();
            Assert.Empty(sut.EdgeType);

            bool res1 = sut.TrySetEdgeType();
            Assert.Empty(sut.EdgeType);
            Assert.False(res1);

            bool ret2 = sut.TrySetEdgeType(typeof(NonDirectionalEdge).Name);
            Assert.Equal(typeof(NonDirectionalEdge).Name, sut.EdgeType);
            Assert.True(ret2);
        }

        [Fact]
        public void SetEdgeTypeEstimationTest()
        {
            // Setting by estimation
            var sut = new Graph();
            Assert.Empty(sut.EdgeType);

            sut.AddEdge(new NonDirectionalEdge(0, 1), (int index) => new CoreNode(index));
            bool ret = sut.TrySetEdgeType();
            Assert.True(ret);
            Assert.Equal(typeof(NonDirectionalEdge).Name, sut.EdgeType);
        }

        [Fact]
        public void SetEdgeTypeInitTest()
        {
            // Setting on initialization
            var edges = new List<IEdge>() { new NonDirectionalEdge(0, 1) };
            var sut = new Graph(edges, (int index) => new CoreNode(index));
            Assert.Equal(typeof(NonDirectionalEdge).Name, sut.EdgeType);
        }

        [Fact]
        public void SetEdgeTypeInitEmptyTest()
        {
            // Setting on initialization (but it is failed because edges and nodes are empty)
            var emptyEdges = new List<IEdge>() { };
            var emptyNodes = new List<INode>() { };
            var sut = new Graph(emptyEdges, emptyNodes);
            Assert.Empty(sut.EdgeType);
        }

        [Fact]
        public void SetNodeTypeTest()
        {
            // Manual setting
            var sut = new Graph();
            Assert.Empty(sut.NodeType);

            bool res1 = sut.TrySetNodeType();
            Assert.Empty(sut.NodeType);
            Assert.False(res1);

            bool ret2 = sut.TrySetNodeType(typeof(CoreNode).Name);
            Assert.Equal(typeof(CoreNode).Name, sut.NodeType);
            Assert.True(ret2);
        }

        [Fact]
        public void SetNodeTypeEstimationTest()
        {
            // Setting by estimation
            var sut = new Graph();
            Assert.Empty(sut.NodeType);

            sut.AddEdge(new NonDirectionalEdge(0, 1), (int index) => new CoreNode(index));
            bool ret = sut.TrySetNodeType();
            Assert.True(ret);
            Assert.Equal(typeof(CoreNode).Name, sut.NodeType);
        }

        [Fact]
        public void SetNodeTypeInitTest()
        {
            // Setting on initialization
            var edges = new List<IEdge>() { new NonDirectionalEdge(0, 1) };
            var sut = new Graph(edges, (int index) => new CoreNode(index));
            Assert.Equal(typeof(CoreNode).Name, sut.NodeType);
        }

        [Fact]
        public void SetNodeTypeInitEmptyTest()
        {
            // Setting on initialization (but it is failed because edges and nodes are empty)
            var emptyEdges = new List<IEdge>() { };
            var emptyNodes = new List<INode>() { };
            var sut = new Graph(emptyEdges, emptyNodes);
            Assert.Empty(sut.NodeType);
        }

        [Fact]
        public void SetNodeTypeInitEmptyTest2()
        {
            // Setting on initialization (but it is failed because edges and nodes are empty)
            var emptyEdges = new List<IEdge>() { };
            var sut = new Graph(emptyEdges, (int index) => new CoreNode(index));
            Assert.Empty(sut.NodeType);
        }
    }
}
