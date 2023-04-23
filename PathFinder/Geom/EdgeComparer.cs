namespace PathFinder.Geom
{
    public class CostComparer : IComparer<IEdge>
    {
        public int Compare(IEdge? x, IEdge? y)
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
