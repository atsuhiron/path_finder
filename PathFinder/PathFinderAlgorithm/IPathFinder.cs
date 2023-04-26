using PathFinder.Geom;

namespace PathFinder.PathFinderAlgorithm
{
    public interface IPathFinder
    {
        public Graph Nodes { get; set; }
        public Route FindRoute(int start, int end);
    }
}
