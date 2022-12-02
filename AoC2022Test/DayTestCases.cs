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


        public void DayStarter()
        {
            var d = new DayStarter();
            Assert.AreEqual(-1, d.Part1());
            Assert.AreEqual(-1, d.Part2());
        }
    }
}