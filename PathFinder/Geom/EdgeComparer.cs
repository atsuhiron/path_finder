namespace PathFinder.Geom
{
    public class EdgeCostComparer<TEdge> : IComparer<TEdge>
        where TEdge : IEdge
    {
        public int Compare(TEdge? x, TEdge? y)
        {
            if (x == null && y == null) return 0;
            if (x == null) return -1;
            if (y == null) return 1;

            if (x.Cost == y.Cost) return 0;
            if (x.Cost < y.Cost) return -1;
            return 1;
        }
    }
}
