using PathFinder.Geom;

namespace PathFinder.PathFinderAlgorithm
{
    public class Dijkstra<TEdge, TNode> : IPathFinder<TEdge, TNode>
        where TEdge : IEdge
        where TNode : class, INode
    {
        protected static readonly MinCostMemoComparer s_minCostMemoComparer = new();

        protected const int MAX_ITER = 10000;

        public Graph<TEdge, TNode> Graph { get; set; }

        public Dijkstra(Graph<TEdge, TNode> graph)
        {
            this.Graph = graph;
        }

        public virtual Route FindRoute(int start, int end)
        {
            if (! ContainNodeInGraph(start, end)) return new Route(0);
            if (start == end) return new Route(start, 0);

            var nodeCount = Graph.GetNodeCount();
            var priorityQueue = new PriorityQueue<int, MinCostMemo>(s_minCostMemoComparer);
            priorityQueue.Enqueue(start, new MinCostMemo(0f, null));
            int iterCount = 0;

            var costs = new Dictionary<int, MinCostMemo>(this.Graph.Nodes.Select(n => KeyValuePair.Create(n.Index, new MinCostMemo())));
            var visited = new Dictionary<int, bool>(this.Graph.Nodes.Select(n => KeyValuePair.Create(n.Index, false)));
            TNode? endNode = Graph.Nodes.Find(n => n.Index == end);

            while (priorityQueue.TryDequeue(out int nodeIndex, out var currentMinCostMemo))
            {
                if (iterCount >= MAX_ITER) return new Route(MAX_ITER);
                if (visited[nodeIndex]) continue;

                costs[nodeIndex] = currentMinCostMemo;
                visited[nodeIndex] = true;

                if (nodeIndex == end) break;

                foreach (var adjIndex in this.Graph.GetAdjacencies(nodeIndex))
                {
                    var edge = this.Graph.SearchEdge(nodeIndex, adjIndex);
                    priorityQueue.Enqueue(adjIndex, new MinCostMemo(CalcCost(edge, currentMinCostMemo.Cost, endNode), nodeIndex));
                }
                iterCount++;
            }

            var edges = BackwardRoute(costs, start, end);
            if (edges == null) return new Route(iterCount);
            return new Route(edges.Select(e => (IEdge)e).ToList(), iterCount);
        }

        protected virtual float CalcCost(TEdge edge, float initValue = 0f, TNode? end = null)
        {
            return initValue + edge.Cost;
        }

        protected List<TEdge>? BackwardRoute(in Dictionary<int, MinCostMemo> minCostDict, int start, int end)
        {
            var edges = new List<TEdge>();
            if (start == end) return edges;

            var currentIndex = end;
            int? beforeIndex = null;

            while (beforeIndex != start)
            {
                if (! minCostDict.ContainsKey(currentIndex)) return null;

                beforeIndex = minCostDict[currentIndex].PreviousNode;

                if (beforeIndex == null)
                {
                    return null;
                }

                edges.Add(Graph.SearchEdge(currentIndex, (int)beforeIndex));
                currentIndex = (int)beforeIndex;
            }

            edges.Reverse();
            return edges;
        }

        protected bool ContainNodeInGraph(int nodeIndex1, int nodeIndex2)
        {
            var indices = Graph.GetNodeIndices();
            return indices.Contains(nodeIndex1) && indices.Contains(nodeIndex2);
        }
    }
}
