namespace PathFinder.PathFinderAlgorithm
{
    public class MinCostMemo
    {
        public float Cost { get; private set; }
        public int? PreviousNode { get; private set; }

        public MinCostMemo()
        {
            Cost = float.MaxValue;
            PreviousNode = null;
        }

        public MinCostMemo(float cost, int? previusnode)
        {
            Cost = cost;
            PreviousNode = previusnode;
        }

        public override string ToString()
        {
            var nodeIndexString = PreviousNode?.ToString() ?? "NULL";
            return $"MCM(cost: {Cost}, prev: {nodeIndexString})";
        }
    }
}
