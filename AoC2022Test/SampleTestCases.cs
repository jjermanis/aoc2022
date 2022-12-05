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
    }
}