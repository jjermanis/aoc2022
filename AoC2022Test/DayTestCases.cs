using AoC2022;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC2022Test
{
    [TestClass]
    public class DayTestCases
    {
        [TestMethod]
        public void Day01()
        {
            var d = new Day01();
            Assert.AreEqual(64929, d.MaxElfCalories());
            Assert.AreEqual(193697, d.TopThreeElfCalories());
        }

        [TestMethod]
        public void Day02()
        {
            var d = new Day02();
            Assert.AreEqual(14531, d.PlanAScore()); 
            Assert.AreEqual(11258, d.PlanBScore());
        }

        [TestMethod]
        public void Day03()
        {
            var d = new Day03();
            Assert.AreEqual(8072, d.SumMisplacedItems());
            Assert.AreEqual(2567, d.SumBadges());
        }

        [TestMethod]
        public void Day04()
        {
            var d = new Day04();
            Assert.AreEqual(490, d.FullyContainedCount());
            Assert.AreEqual(921, d.OverlapCount());
        }

        [TestMethod]
        public void Day05()
        {
            var d = new Day05();
            Assert.AreEqual("VWLCWGSDQ", d.SingleCrateSummary());
            Assert.AreEqual("TCGLQSLPW", d.MultiCrateSummary());
        }

        [TestMethod]
        public void Day06()
        {
            var d = new Day06();
            Assert.AreEqual(1625, d.StartOfPacket());
            Assert.AreEqual(2250, d.StartOfMessage());
        }

        [TestMethod]
        public void Day07()
        {
            var d = new Day07();
            Assert.AreEqual(1367870, d.SmallDirectoryTotalSize());
            Assert.AreEqual(549173, d.OptimalDirectoryDeletionSize());
        }

        [TestMethod]
        public void Day08()
        {
            var d = new Day08();
            Assert.AreEqual(1715, d.VisibleTreeCount());
            Assert.AreEqual(374400, d.BestScenicScore());
        }

        [TestMethod]
        public void Day09()
        {
            var d = new Day09();
            Assert.AreEqual(6503, d.UniqueTailPositions2());
            Assert.AreEqual(2724, d.UniqueTailPositions10());
        }

        [TestMethod]
        public void Day10()
        {
            var d = new Day10();
            Assert.AreEqual(14360, d.InterestingSignalSum());
            // TODO: figure out a unit test for this one
        }

        [TestMethod]
        public void Day11()
        {
            var d = new Day11();
            Assert.AreEqual(102399, d.MonkeyBusinessLevelShort());
            Assert.AreEqual(23641658401, d.MonkeyBusinessLevelLong());
        }

        public void DayStarter()
        {
            var d = new DayStarter();
            Assert.AreEqual(-1, d.Part1());
            Assert.AreEqual(-1, d.Part2());
        }
    }
}