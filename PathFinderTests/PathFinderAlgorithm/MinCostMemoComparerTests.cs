using PathFinder.PathFinderAlgorithm;

namespace PathFinderTests.PathFinderAlgorithm
{
    public class MinCostMemoComparerTests
    {
        [Fact]
        public void MinCostMemoComparerTest()
        {
            var memoList = new List<MinCostMemo>()
            {
                new MinCostMemo(24f, 5),
                new MinCostMemo(0f, 0),
                new MinCostMemo(4.5f, 2),
                new MinCostMemo(0.5f, 1),
                new MinCostMemo(13f, 4),
                new MinCostMemo(9.9f, 3)
            };

            memoList.Sort(new  MinCostMemoComparer());
            Assert.Equal(0, memoList[0].PreviousNode);
            Assert.Equal(1, memoList[1].PreviousNode);
            Assert.Equal(2, memoList[2].PreviousNode);
            Assert.Equal(3, memoList[3].PreviousNode);
            Assert.Equal(4, memoList[4].PreviousNode);
            Assert.Equal(5, memoList[5].PreviousNode);
        }
    }
}
