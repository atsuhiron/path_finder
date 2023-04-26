using PathFinder.Geom;

namespace PathFinder.PathFinderAlgorithm
{
    public class Dijkstra : IPathFinder
    {
        private static readonly MinCostMemoComparer s_minCostMemoComparer = new();

        private const int MAX_ITER = 10000;

        public Graph Graph { get; set; }

        public Dijkstra(Graph graph)
        {
            this.Graph = graph;
        }

        public Route FindRoute(int start, int end)
        {
            var nodeCount = Graph.GetNodeCount();
            var priorityQueue = new PriorityQueue<int, MinCostMemo>(s_minCostMemoComparer);
            priorityQueue.Enqueue(start, new MinCostMemo(0f, null));
            int iterCount = 0;

            var costs = new Dictionary<int, MinCostMemo>(this.Graph.NodeIndices.Select(index => KeyValuePair.Create(index, new MinCostMemo())));
            var visited = new Dictionary<int, bool>(this.Graph.NodeIndices.Select(index => KeyValuePair.Create(index, false)));

            while (priorityQueue.TryDequeue(out int nodeIndex, out var currentMinCostMemo))
            {
                if (iterCount >= MAX_ITER) return new Route(MAX_ITER);
                if (visited[nodeIndex]) continue;

                costs[nodeIndex] = currentMinCostMemo;
                visited[nodeIndex] = true;

                foreach (var adjIndex in this.Graph.GetAdjacencies(nodeIndex))
                {
                    var edge = this.Graph.SearchEdge(nodeIndex, adjIndex);
                    priorityQueue.Enqueue(adjIndex, new MinCostMemo(currentMinCostMemo.Cost + edge.Cost, nodeIndex));
                }
                iterCount++;
            }

            // TODO: cost -> route
            var edges = BackwardRoute(costs, start, end);
            if (edges == null) return new Route(iterCount);
            return new Route(edges, iterCount);
        }

        private List<IEdge>? BackwardRoute(in Dictionary<int, MinCostMemo> minCostDict, int start, int end)
        {
            var edges = new List<IEdge>();
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
    }
}
