using PathFinder.Geom;

namespace PathFinder.PathFinderAlgorithm
{
    public interface IPathFinder
    {
        Route FindRoute(int start, int end);
    }
}
