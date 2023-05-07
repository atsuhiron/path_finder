namespace PathFinder.Geom
{
    public class Graph
    {
        private static readonly EdgeCostComparer<IEdge> s_costComparer = new();
        private static readonly NodeIndexComparer<INode> s_nodeIndexComparer = new();

        public string EdgeType { get; private set; }
        public string NodeType { get; private set; }

        public List<IEdge> Edges { get; init; }
        public List<INode> Nodes { get; init; }

        public Graph()
        {
            EdgeType = string.Empty;
            NodeType = string.Empty;

            Edges = new List<IEdge>();
            Nodes = new List<INode>();
        }

        public Graph(List<IEdge> edges, Func<int, INode> constructor)
        {
            EdgeType = edges.FirstOrDefault()?.GetType().Name ?? string.Empty;

            Edges = new HashSet<IEdge>(edges).ToList();
            var _ni = new HashSet<int>(edges.Select(e => e.Start));
            _ni.UnionWith(edges.Select(e => e.End));
            
            var nodeIndices = _ni.ToList();
            Nodes = _ni.Select(ni => constructor(ni)).ToList();
            Nodes.Sort(s_nodeIndexComparer);
            NodeType = Nodes.FirstOrDefault()?.GetType().Name ?? string.Empty;
        }

        public Graph(List<IEdge> edges, List<INode> nodes)
        {
            EdgeType = edges.FirstOrDefault()?.GetType().Name ?? string.Empty;
            NodeType = nodes.FirstOrDefault()?.GetType().Name ?? string.Empty;

            Edges = new HashSet<IEdge>(edges).ToList();
            Nodes = nodes;
            Nodes.Sort(s_nodeIndexComparer);
            if (! CheckAllEdge())
            {
                throw new ArgumentException("未定義の Node が Edge の定義に含まれています。");
            }
        }

        public bool TrySetEdgeType(string edgeTypeName = "")
        {
            if (string.IsNullOrEmpty(edgeTypeName))
            {
                EdgeType = Edges.FirstOrDefault()?.GetType().Name ?? string.Empty;
            }
            else
            {
                EdgeType = edgeTypeName;
            }

            return !string.IsNullOrEmpty(EdgeType);
        }

        public bool TrySetNodeType(string nodeTypeName = "")
        {
            if (string.IsNullOrEmpty(nodeTypeName))
            {
                NodeType = Nodes.FirstOrDefault()?.GetType().Name ?? string.Empty;
            }
            else
            {
                NodeType = nodeTypeName;
            }

            return !string.IsNullOrEmpty(NodeType);
        }

        public List<int> GetNodeIndices() => Nodes.Select(n => n.Index).ToList();

        public void AddEdge(IEdge edge)
        {
            if (Edges.Contains(edge)) return;

            if (!CheckEdge(edge))
            {
                throw new ArgumentException("未定義の Node が Edge の定義に含まれています。");
            }

            Edges.Add(edge);
        }

        public void AddEdge(IEdge edge, Func<int, INode> constructor)
        {
            if (Edges.Contains(edge)) return;

            if (CheckEdge(edge))
            {
                Edges.Add(edge);
                return;
            }

            if (!ContainNode(edge.Start)) Nodes.Add(constructor(edge.Start));
            if (!ContainNode(edge.End)) Nodes.Add(constructor(edge.End));
            Nodes.Sort(s_nodeIndexComparer);
            Edges.Add(edge);
        }

        public List<int> GetAdjacencies(int index)
        {
            // TODO: あらかじめ隣接行列を定義した方が良いかも
            var adjFrom = new HashSet<int>(Edges.Where(e => e.Start == index).Select(e => e.End));
            var adjTo = new HashSet<int>(Edges.Where(e => e.End == index).Select(e => e.Start));
            var adj = adjFrom.Union(adjTo).ToList();
            adj.Sort();
            return adj;
        }

        public IEdge SearchEdge(int nodeIndex1, int nodeIndex2)
        {
            var edge = Edges.Where(e => ((e.Start == nodeIndex1) && (e.End == nodeIndex2)) || (e.Start == nodeIndex2) && (e.End == nodeIndex1)).ToList();
            
            if (edge.Count == 0) throw new ArgumentException("指定された Edge が存在しません");
            if (edge.Count == 1) return edge.First();
            edge.Sort(s_costComparer);
            return edge.First();
        }

        public int GetNodeCount() => Nodes.Count;

        private bool CheckAllEdge()
        {
            var nodeSet = new HashSet<int>(GetNodeIndices());
            return Edges.All(e => nodeSet.Contains(e.Start)) && Edges.All(e => nodeSet.Contains(e.End));
        }

        private bool CheckEdge(IEdge edge)
        {
            return ContainNode(edge.Start) && ContainNode(edge.End);
        }

        private bool ContainNode(int index) => Nodes.Any(n => n.Index == index);

        public static Graph CreateGrid(int x, int y)
        {
            var edges = new List<NonDirectionalEdge>();

            foreach (int xi in Enumerable.Range(0, x))
            {
                foreach (int yi in Enumerable.Range(0, y))
                {
                    var cur = x * yi + xi;
                    if (xi != x - 1) edges.Add(new NonDirectionalEdge(cur, cur + 1));
                    if (yi != y - 1) edges.Add(new NonDirectionalEdge(cur, cur + x));
                }
            }

            return new Graph(
                edges.Select(e => (IEdge)e).ToList(),
                (int index) => new CoreNode(index)
            );
        }

        public static Graph CreateXYGrid(int x, int y)
        {
            var edges = new List<NonDirectionalEdge>();
            var nodes = new List<XYNode>();
            int counter = 0;

            foreach (int yi in Enumerable.Range(0, y))
            {
                foreach (int xi in Enumerable.Range(0, x))
                {
                    nodes.Add(new XYNode(counter, xi, yi));

                    var cur = x * yi + xi;
                    if (xi != x - 1) edges.Add(new NonDirectionalEdge(cur, cur + 1));
                    if (yi != y - 1) edges.Add(new NonDirectionalEdge(cur, cur + x));
                    counter++;
                }
            }

            return new Graph(
                edges.Select(e => (IEdge)e).ToList(),
                nodes.Select(n => (INode)n).ToList()
            );
        }
    }
}
