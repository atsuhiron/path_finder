namespace PathFinder.Geom
{
    public class NodeIndexComparer : IComparer<INode>
    {
        public int Compare(INode? x, INode? y)
        {
            if (x == null && y == null) return 0;
            if (x == null) return -1;
            if (y == null) return 1;

            if (x.Index == y.Index) return 0;
            if (x.Index < y.Index) return -1;
            return 1;
        }
    }
}
