namespace PathFinder.Geom
{
    public class Route
    {
        public List<int> RouteNodeIndices { get; init; }
        public List<IEdge> RouteEdges { get; init; }
        public bool Success { get; init; }
        public int Iteration { get; init; }

        public Route(List<IEdge> routeEdges, int iter)
        {
            if (!ValidateEdgeContinuity(routeEdges))
            {
                throw new ArgumentException("Node の連続性が保たれてません。");
            }
            RouteEdges = routeEdges;
            
            RouteNodeIndices = routeEdges.Select(e => e.Start).ToList();
            RouteNodeIndices.Add(routeEdges.Last().End);
            Success = true;
            Iteration = iter;
        }

        public Route(int iter)
        {
            RouteEdges = new List<IEdge>();
            RouteNodeIndices = new List<int>();
            Success = false;
            Iteration = iter;
        }

        public float SumCost() => RouteEdges.Select(e => e.Cost).Sum();

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
