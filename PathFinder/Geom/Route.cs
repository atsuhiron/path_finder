namespace PathFinder.Geom
{
    public class Route
    {
        public List<int> RouteNodeIndices { get; init; }
        public List<IEdge> RouteEdges { get; init; }

        public Route(List<IEdge> routeEdges)
        {
            if (!ValidateEdgeContinuity(routeEdges))
            {
                throw new ArgumentException("Node の連続性が保たれてません。");
            }
            RouteEdges = routeEdges;
            
            RouteNodeIndices = routeEdges.Select(e => e.Start).ToList();
            RouteNodeIndices.Add(routeEdges.Last().End);
        }

        public int SumCost() => RouteEdges.Select(e => e.Cost).Sum();

        private static bool ValidateEdgeContinuity(List<IEdge> routeEdges)
        {
            for (int i = 0; i < routeEdges.Count - 1; i++)
            {
                if (routeEdges[i].End != routeEdges[i + 1].Start) return false;
            }
            return true;
        }
    }
}
