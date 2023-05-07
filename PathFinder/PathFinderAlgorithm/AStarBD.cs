using PathFinder.Geom;

namespace PathFinder.PathFinderAlgorithm
{
    public class AStarBD<TNode, TEdge> : AStar<TNode>
        where TNode : class, INode, IXYCoordinated
        where TEdge : NonDirectionalEdge
    {
        public AStarBD(Graph graph) : base(graph) {}

        public AStarBD(Graph graph, CalcCostCoreDelegate calcCostCoreDelegate) : base(graph, calcCostCoreDelegate) { }

        public override Route FindRoute(int start, int end)
        {
            if (!ContainNodeInGraph(start, end)) return new Route(0);
            if (start == end) return new Route(start, 0);

            var nodeIndices = Graph.GetNodeIndices();
            int iterCount = 0;
            INode? startNode = Graph.Nodes.Find(n => n.Index == start);
            INode? endNode = Graph.Nodes.Find(n => n.Index == end);

            // Foreward
            var priorityQueueForeward = new PriorityQueue<int, MinCostMemo>(s_minCostMemoComparer);
            priorityQueueForeward.Enqueue(start, new MinCostMemo(0f, null));
            var costsForeward = new Dictionary<int, MinCostMemo>(this.Graph.Nodes.Select(n => KeyValuePair.Create(n.Index, new MinCostMemo())));
            var visitedForeward = new Dictionary<int, bool>(this.Graph.Nodes.Select(n => KeyValuePair.Create(n.Index, false)));

            // Backward
            var priorityQueueBackward = new PriorityQueue<int, MinCostMemo>(s_minCostMemoComparer);
            priorityQueueBackward.Enqueue(end, new MinCostMemo(0f, null));
            var costsBackward = new Dictionary<int, MinCostMemo>(this.Graph.Nodes.Select(n => KeyValuePair.Create(n.Index, new MinCostMemo())));
            var visitedBackward = new Dictionary<int, bool>(this.Graph.Nodes.Select(n => KeyValuePair.Create(n.Index, false)));

            while (
                priorityQueueForeward.TryDequeue(out int nodeIndexForeward, out var currentMinCostMemoForeward) &&
                priorityQueueBackward.TryDequeue(out int nodeIndexBackward, out var currentMinCostMemoBackward)
            )
            {
                if (iterCount >= MAX_ITER) return new Route(MAX_ITER);
                if (!visitedForeward[nodeIndexForeward])
                {
                    costsForeward[nodeIndexForeward] = currentMinCostMemoForeward;
                    if (visitedBackward[nodeIndexForeward])
                    {
                        // 終了処理
                        return Postprocess(costsForeward, costsBackward, start, end, nodeIndexForeward, iterCount);
                    }

                    visitedForeward[nodeIndexForeward] = true;

                    foreach (var adjIndex in this.Graph.GetAdjacencies(nodeIndexForeward))
                    {
                        var edge = this.Graph.SearchEdge(nodeIndexForeward, adjIndex);
                        priorityQueueForeward.Enqueue(adjIndex, new MinCostMemo(CalcCost(edge, currentMinCostMemoForeward.Cost, endNode), nodeIndexForeward));
                    }
                }

                if (!visitedBackward[nodeIndexBackward])
                {
                    costsBackward[nodeIndexBackward] = currentMinCostMemoBackward;
                    if (visitedForeward[nodeIndexBackward])
                    {
                        // 終了処理
                        return Postprocess(costsForeward, costsBackward, start, end, nodeIndexBackward, iterCount);
                    }

                    visitedBackward[nodeIndexBackward] = true;

                    foreach (var adjIndex in this.Graph.GetAdjacencies(nodeIndexBackward))
                    {
                        var edge = this.Graph.SearchEdge(nodeIndexBackward, adjIndex);
                        priorityQueueBackward.Enqueue(adjIndex, new MinCostMemo(CalcCost(edge, currentMinCostMemoBackward.Cost, startNode), nodeIndexBackward));
                    }
                }

                iterCount++;
            }
            return new Route(iterCount);
        }

        private Route Postprocess(in Dictionary<int, MinCostMemo> costsF, in Dictionary<int, MinCostMemo> costsB, int start, int end, int midNodeIndex, int iterCount)
        {
            var edgesForeward = BackwardRoute(costsF, start, midNodeIndex) ?? new List<IEdge>();
            var edgesBackward = BackwardRoute(costsB, end, midNodeIndex) ?? new List<IEdge>();
            edgesBackward.Reverse();

            edgesForeward.AddRange(edgesBackward);

            if (TryAlignEdgeDirection(edgesForeward, out List<IEdge> edges))
            {
                return new Route(edges, iterCount);
            }
            return new Route(iterCount);
        }
    }
}
