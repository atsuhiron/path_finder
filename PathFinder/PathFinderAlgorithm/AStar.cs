using PathFinder.Geom;

namespace PathFinder.PathFinderAlgorithm
{
    public class AStar<TEdge, TNode> : Dijkstra<TEdge, TNode>
        where TEdge : IEdge
        where TNode : class, INode, IXYCoordinated
    {
        public delegate float CalcCostCoreDelegate(float edgeCost, float deltaX, float deltaY);

        private readonly CalcCostCoreDelegate CalcCostCore;

        public AStar(Graph<TEdge, TNode> graph) : base(graph)
        {
            CalcCostCore = L2Heuristics;
        }

        public AStar(Graph<TEdge, TNode> graph, CalcCostCoreDelegate calcCostCoreDelegate) : base(graph)
        {
            CalcCostCore = calcCostCoreDelegate;
        }

        protected override float CalcCost(TEdge edge, float initValue = 0, TNode? end = null)
        {
            if (end == null)
            {
                return base.CalcCost(edge, initValue);
            }

            var nextNode = Graph.Nodes.Find(n => n.Index == edge.End);
            var currNode = Graph.Nodes.Find(n => n.Index == edge.Start);
            if (nextNode == null || currNode == null)
            {
                return base.CalcCost(edge, initValue);
            }

            var deltaX = nextNode.X - currNode.X;
            var deltaY = nextNode.Y - currNode.Y;
            return CalcCostCore(edge.Cost, deltaX, deltaY);
        }

        private static float L1Heuristics(float edgeCost, float deltaX, float deltaY)
        {
            return edgeCost + MathF.Abs(deltaX) + MathF.Abs(deltaY);
        }

        private static float L2Heuristics(float edgeCost, float deltaX, float deltaY)
        {
            return edgeCost + MathF.Sqrt(deltaX*deltaX + deltaY*deltaY);
        }
    }
}
