namespace PathFinder.Geom
{
    public class Graph
    {
        private static readonly EdgeCostComparer s_costComparer = new();
        private static readonly NodeIndexComparer s_nodeIndexComparer = new();

        public List<IEdge> Edges { get; init; }
        public List<INode> Nodes { get; init; }

        public Graph()
        {
            Edges = new List<IEdge>();
            Nodes = new List<INode>();
        }

        public Graph(List<IEdge> edges)
        {
            Edges = new HashSet<IEdge>(edges).ToList();
            var _ni = new HashSet<int>(edges.Select(e => e.Start));
            _ni.UnionWith(edges.Select(e => e.End));
            
            var nodeIndices = _ni.ToList();
            Nodes = _ni.ToList();
            Nodes.Sort();
        }

        public Graph(List<IEdge> edges, List<int> nodeIndices)
        {
            Edges = new HashSet<IEdge>(edges).ToList();
            Nodes = nodeIndices;
            Nodes.Sort();
            if (! CheckAllEdge())
            {
                throw new ArgumentException("未定義の Node が Edge の定義に含まれています。");
            }
        }

        public void AddEdge(IEdge edge, bool addNode)
        {
            if (Edges.Contains(edge)) return;

            if (CheckEdge(edge))
            {
                Edges.Add(edge);
                return;
            }

            if (! addNode)
            {
                throw new ArgumentException("未定義の Node が Edge の定義に含まれています。");
            }

            if (! Nodes.Contains(edge.Start)) Nodes.Add(edge.Start);
            if (! Nodes.Contains(edge.End)) Nodes.Add(edge.End);
            Nodes.Sort();
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
            var nodeSet = new HashSet<int>(Nodes);
            return Edges.All(e => nodeSet.Contains(e.Start)) && Edges.All(e => nodeSet.Contains(e.End));
        }

        private bool CheckEdge(IEdge edge)
        {
            return Nodes.Contains(edge.Start) && Nodes.Contains(edge.End);
        }

        public static Graph CreateGrid(int x, int y)
        {
            var edges = new List<IEdge>();

            foreach (int xi in Enumerable.Range(0, x))
            {
                foreach (int yi in Enumerable.Range(0, y))
                {
                    var cur = x * yi + xi;
                    if (xi != x - 1) edges.Add(new NonDirectionalEdge(cur, cur + 1));
                    if (yi != y - 1) edges.Add(new NonDirectionalEdge(cur, cur + x));
                }
            }

            return new Graph(edges);
        }
    }
}
