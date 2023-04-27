using PathFinder.Geom;

namespace PathFinder.PathFinderAlgorithm
{
    public interface IPathFinder<TEdge, TNode>
        where TEdge : IEdge
        where TNode : INode
    {
        public Graph<TEdge, TNode> Graph { get; set; }
        public Route FindRoute(int start, int end);
    }
}
