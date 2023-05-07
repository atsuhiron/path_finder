using PathFinder.Geom;

namespace PathFinder.PathFinderAlgorithm
{
    public interface IPathFinder
    {
        public Graph Graph { get; set; }
        public Route FindRoute(int start, int end);
    }
}
