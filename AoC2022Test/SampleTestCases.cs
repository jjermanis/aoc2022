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


    }
}