using AoC2022;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC2022Test
{
    [TestClass]
    public class SampleTestCases
    {
        [TestMethod]
        public void Day01()
        {
            var d = new Day01("Day01Test.txt");
            Assert.AreEqual(24000, d.MaxElfCalories());
            Assert.AreEqual(45000, d.TopThreeElfCalories());
        }

        [TestMethod]
        public void Day02()
        {
            var d = new Day02("Day02Test.txt");
            Assert.AreEqual(15, d.PlanAScore());
            Assert.AreEqual(12, d.PlanBScore());
        }

        [TestMethod]
        public void Day03()
        {
            var d = new Day03("Day03Test.txt");
            Assert.AreEqual(157, d.SumMisplacedItems());
            Assert.AreEqual(70, d.SumBadges());
        }

        [TestMethod]
        public void Day04()
        {
            var d = new Day04("Day04Test.txt");
            Assert.AreEqual(2, d.FullyContainedCount());
            Assert.AreEqual(4, d.OverlapCount());
        }

        [TestMethod]
        public void Day05()
        {
            var d = new Day05("Day05Test.txt");
            Assert.AreEqual("CMZ", d.SingleCrateSummary());
            Assert.AreEqual("MCD", d.MultiCrateSummary());
        }

        [TestMethod]
        public void Day06()
        {
            var d = new Day06("Day06Test.txt");
            Assert.AreEqual(7, d.StartOfPacket());
            Assert.AreEqual(19, d.StartOfMessage());

            var d2 = new Day06("Day06Test2.txt");
            Assert.AreEqual(5, d2.StartOfPacket());
            Assert.AreEqual(23, d2.StartOfMessage());

            var d3 = new Day06("Day06Test3.txt");
            Assert.AreEqual(6, d3.StartOfPacket());
            Assert.AreEqual(23, d3.StartOfMessage());

            var d4 = new Day06("Day06Test4.txt");
            Assert.AreEqual(10, d4.StartOfPacket());
            Assert.AreEqual(29, d4.StartOfMessage());

            var d5 = new Day06("Day06Test5.txt");
            Assert.AreEqual(11, d5.StartOfPacket());
            Assert.AreEqual(26, d5.StartOfMessage());
        }

        [TestMethod]
        public void Day07()
        {
            var d = new Day07("Day07Test.txt");
            Assert.AreEqual(95437, d.SmallDirectoryTotalSize());
            Assert.AreEqual(24933642, d.OptimalDirectoryDeletionSize());
        }
    }
}