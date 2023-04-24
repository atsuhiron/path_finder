using PathFinder.Geom;

namespace PathFinder.PathFinderAlgorithm
{
    public class Dijkstra : IPathFinder
    {
        private const int MAX_ITER = 10000;

        public Nodes Nodes { get; set; }

        public Dijkstra(Nodes nodes)
        {
            this.Nodes = nodes;
        }

        public Route FindRoute(int start, int end)
        {
            //var _kvPairsCost = this.Nodes.NodeIndices.Select(index => KeyValuePair.Create(index, float.MaxValue));
            //var _kvPairsVisited = this.Nodes.NodeIndices.Select(index => KeyValuePair.Create(index, false));
            //var minimumCost = new Dictionary<int, float>(_kvPairsCost)
            //{
            //    [start] = 0f
            //};
            //var visited = new Dictionary<int, bool>(_kvPairsVisited);
            //var nodeIndices = new HashSet<int>(this.Nodes.NodeIndices);

            //var route = new List<IEdge>();
            //int iterCount = 0;

            //while (nodeIndices.Count > 0 && iterCount <= MAX_ITER)
            //{
            //    (int poppedIndex, nodeIndices) = PopMinimumFromList(nodeIndices, minimumCost);
            //    if (visited[poppedIndex]) continue;
            //    visited[poppedIndex] = true;

            //    var adj = this.Nodes.GetAdjacencies(poppedIndex);
            //    foreach (var adjNodeIndex in adj)
            //    {
            //        var edge = this.Nodes.GetEdge(poppedIndex, adjNodeIndex);
            //        if (minimumCost[adjNodeIndex] > minimumCost[poppedIndex] + edge.Cost)
            //        {
            //            minimumCost[adjNodeIndex] = minimumCost[poppedIndex] + edge.Cost;
            //            route.Add(edge);
            //        }
            //    }

            //    iterCount++;
            //}

            //return new Route(route, iterCount);

            var nodeCount = Nodes.GetNodeCount();
            var priorityQueue = new PriorityQueue<int, float>();
            priorityQueue.Enqueue(start, 0f);
            int iterCount = 0;

            var costs = new Dictionary<int, float>(this.Nodes.NodeIndices.Select(index => KeyValuePair.Create(index, float.MaxValue)));
            var visited = new Dictionary<int, bool>(this.Nodes.NodeIndices.Select(index => KeyValuePair.Create(index, false)));

            while (priorityQueue.TryDequeue(out int nodeIndex, out float currentCost))
            {
                if (iterCount >= MAX_ITER) return new Route(MAX_ITER);
                if (visited[nodeIndex]) continue;

                costs[nodeIndex] = currentCost;
                visited[nodeIndex] = true;

                foreach (var adjIndex in this.Nodes.GetAdjacencies(nodeIndex))
                {
                    var edge = this.Nodes.GetEdge(nodeIndex, adjIndex);
                    priorityQueue.Enqueue(adjIndex, currentCost + edge.Cost);
                }
                iterCount++;
            }

            // TODO: cost -> route
            return new Route(iterCount);
        }

        private static (int, HashSet<int>) PopMinimumFromList(HashSet<int> nodeIndices, in Dictionary<int, float> minimumCost)
        {
            var minCost = float.MaxValue;
            int minCostKey = -1;
            var found = false;
            foreach (var kv in minimumCost)
            {
                if ((kv.Value < minCost) && (nodeIndices.Contains(kv.Key)))
                {
                    minCost = kv.Value;
                    minCostKey = kv.Key;
                    found = true;
                }
            }

            if (!found) throw new ArgumentException("全ての minimumCost が未初期化です。");
            nodeIndices.Remove(minCostKey);
            return (minCostKey, nodeIndices);
        }
    }
}
