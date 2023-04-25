namespace PathFinder.PathFinderAlgorithm
{
    public class MinCostMemoComparer : IComparer<MinCostMemo>
    {
        public int Compare(MinCostMemo? x, MinCostMemo? y)
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
