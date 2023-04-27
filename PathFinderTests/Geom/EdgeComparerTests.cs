using PathFinder.Geom;

namespace PathFinderTests.Geom
{
    public class EdgeComparerTests
    {
        [Fact]
        public void EdgeCostComparerTest()
        {
            var memoList = new List<NonDirectionalEdge>()
            {
                new NonDirectionalEdge(0, 5, 24f),
                new NonDirectionalEdge(0, 0, 0f),
                new NonDirectionalEdge(0, 2, 4.5f),
                new NonDirectionalEdge(0, 1, 0.5f),
                new NonDirectionalEdge(0, 4, 13f),
                new NonDirectionalEdge(0, 3, 9.9f)
            };

            memoList.Sort(new EdgeCostComparer<NonDirectionalEdge>());
            Assert.Equal(0, memoList[0].End);
            Assert.Equal(1, memoList[1].End);
            Assert.Equal(2, memoList[2].End);
            Assert.Equal(3, memoList[3].End);
            Assert.Equal(4, memoList[4].End);
            Assert.Equal(5, memoList[5].End);
        }
    }
}
