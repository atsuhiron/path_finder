﻿using PathFinder.Geom;

namespace PathFinder.PathFinderAlgorithm
{
    public class Dijkstra<TEdge, TNode> : IPathFinder<TEdge, TNode>
        where TEdge : IEdge
        where TNode : INode
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
            if (start == end) return new Route(start, 0);

            var nodeCount = Graph.GetNodeCount();
            var priorityQueue = new PriorityQueue<int, MinCostMemo>(s_minCostMemoComparer);
            priorityQueue.Enqueue(start, new MinCostMemo(0f, null));
            int iterCount = 0;

            var costs = new Dictionary<int, MinCostMemo>(this.Graph.Nodes.Select(n => KeyValuePair.Create(n.Index, new MinCostMemo())));
            var visited = new Dictionary<int, bool>(this.Graph.Nodes.Select(n => KeyValuePair.Create(n.Index, false)));

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
                    priorityQueue.Enqueue(adjIndex, new MinCostMemo(currentMinCostMemo.Cost + edge.Cost, nodeIndex));
                }
                iterCount++;
            }

            var edges = BackwardRoute(costs, start, end);
            if (edges == null) return new Route(iterCount);
            return new Route(edges.Select(e => (IEdge)e).ToList(), iterCount);
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
    }
}
