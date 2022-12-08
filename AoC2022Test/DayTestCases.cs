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

        public void DayStarter()
        {
            var d = new DayStarter();
            Assert.AreEqual(-1, d.Part1());
            Assert.AreEqual(-1, d.Part2());
        }
    }
}