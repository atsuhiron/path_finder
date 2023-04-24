using PathFinder.Geom;

namespace PathFinder.PathFinderAlgorithm
{
    public interface IPathFinder
    {
        public Nodes Nodes { get; set; }
        public Route FindRoute(int start, int end);
    }
}
