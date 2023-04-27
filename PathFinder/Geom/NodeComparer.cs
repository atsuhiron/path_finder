namespace PathFinder.Geom
{
    public class NodeIndexComparer<TNode> : IComparer<TNode>
        where TNode : INode
    {
        public int Compare(TNode? x, TNode? y)
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
