namespace PathFinder.Geom
{
    public class Graph<TEdge, TNode> 
        where TEdge : IEdge
        where TNode : INode
    {
        private static readonly EdgeCostComparer<TEdge> s_costComparer = new();
        private static readonly NodeIndexComparer<TNode> s_nodeIndexComparer = new();

        public List<TEdge> Edges { get; init; }
        public List<TNode> Nodes { get; init; }

        public Graph()
        {
            Edges = new List<TEdge>();
            Nodes = new List<TNode>();
        }

        public Graph(List<TEdge> edges, Func<int, TNode> constructor)
        {
            Edges = new HashSet<TEdge>(edges).ToList();
            var _ni = new HashSet<int>(edges.Select(e => e.Start));
            _ni.UnionWith(edges.Select(e => e.End));
            
            var nodeIndices = _ni.ToList();
            Nodes = _ni.Select(ni => constructor(ni)).ToList();
            Nodes.Sort(s_nodeIndexComparer);
        }

        public Graph(List<TEdge> edges, List<TNode> nodes)
        {
            Edges = new HashSet<TEdge>(edges).ToList();
            Nodes = nodes;
            Nodes.Sort(s_nodeIndexComparer);
            if (! CheckAllEdge())
            {
                throw new ArgumentException("未定義の Node が Edge の定義に含まれています。");
            }
        }

        public List<int> GetNodeIndices() => Nodes.Select(n => n.Index).ToList();

        public void AddEdge(TEdge edge)
        {
            if (Edges.Contains(edge)) return;

            if (!CheckEdge(edge))
            {
                throw new ArgumentException("未定義の Node が Edge の定義に含まれています。");
            }

            Edges.Add(edge);
        }

        public void AddEdge(TEdge edge, Func<int, TNode> constructor)
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

        public TEdge SearchEdge(int nodeIndex1, int nodeIndex2)
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

        private bool CheckEdge(TEdge edge)
        {
            return ContainNode(edge.Start) && ContainNode(edge.End);
        }

        private bool ContainNode(int index) => Nodes.Any(n => n.Index == index);

        public static Graph<NonDirectionalEdge, CoreNode> CreateGrid(int x, int y)
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

            return new Graph<NonDirectionalEdge, CoreNode>(edges, (int index) => new CoreNode(index));
        }
    }
}
