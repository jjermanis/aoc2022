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

        [TestMethod]
        public void Day08()
        {
            var d = new Day08("Day08Test.txt");
            Assert.AreEqual(21, d.VisibleTreeCount());
            Assert.AreEqual(8, d.BestScenicScore());
        }

        [TestMethod]
        public void Day09()
        {
            var d = new Day09("Day09Test.txt");
            Assert.AreEqual(13, d.UniqueTailPositions2());
            Assert.AreEqual(1, d.UniqueTailPositions10());

            var d2 = new Day09("Day09Test2.txt");
            Assert.AreEqual(36, d2.UniqueTailPositions10());
        }

        [TestMethod]
        public void Day10()
        {
            var d = new Day10("Day10Test.txt");
            Assert.AreEqual(13140, d.InterestingSignalSum());
            // TODO: figure out a unit test for this one
        }

        [TestMethod]
        public void Day11()
        {
            var d = new Day11("Day11Test.txt");
            Assert.AreEqual(10605, d.MonkeyBusinessLevelShort());
            Assert.AreEqual(2713310158, d.MonkeyBusinessLevelLong());
        }

        [TestMethod]
        public void Day12()
        {
            var d = new Day12("Day12Test.txt");
            Assert.AreEqual(31, d.PathLengthFromStart());
            Assert.AreEqual(29, d.PathLengthFromLowPoint());
        }

        [TestMethod]
        public void Day13()
        {
            var d = new Day13("Day13Test.txt");
            Assert.AreEqual(13, d.Part1());
            Assert.AreEqual(140, d.Part2());
        }

        [TestMethod]
        public void Day14()
        {
            var d = new Day14("Day14Test.txt");
            Assert.AreEqual(24, d.SoundCountToFloor());
            Assert.AreEqual(93, d.SandCountToSource());
        }

        [TestMethod]
        public void Day15()
        {
            var d = new Day15("Day15Test.txt");
            Assert.AreEqual(26, d.NoBeaconCount(10));
            Assert.AreEqual(56000011, d.DistressTuningFrequency(20));
        }

        [TestMethod]
        public void Day16()
        {
            var d = new Day16("Day16Test.txt");
            Assert.AreEqual(1651, d.MaxPressureSolo());
            Assert.AreEqual(1707, d.Part2());
        }

        [TestMethod]
        public void Day17()
        {
            var d = new Day17("Day17Test.txt");
            Assert.AreEqual(3068, d.SmallTowerHeight());
            //Assert.AreEqual(-1, d.Part2()); // Keep commented for now - runs for half-hour
        }

        [TestMethod]
        public void Day18()
        {
            var d = new Day18("Day18Test.txt");
            Assert.AreEqual(64, d.OpenFaceCount());
            Assert.AreEqual(58, d.ExposedFaceCount());
        }

        [TestMethod]
        public void Day20()
        {
            var d = new Day20("Day20Test.txt");
            Assert.AreEqual(3, d.GroveSum());
            Assert.AreEqual(1623178306, d.DecryptedGroveSum());
        }

        [TestMethod]
        public void Day21()
        {
            var d = new Day21("Day21Test.txt");
            Assert.AreEqual(152, d.MonkeyComputation());
            Assert.AreEqual(301, d.HumanIntervention());
        }

        [TestMethod]
        public void Day22()
        {
            var d = new Day22("Day22Test.txt");
            Assert.AreEqual(6032, d.Password2D());
            Assert.AreEqual(104385, d.Password3D());
        }

        [TestMethod]
        public void Day23()
        {
            var d = new Day23("Day23Test.txt");
            Assert.AreEqual(25, d.SpaceAfter10Moves());
            Assert.AreEqual(4, d.FirstStationaryRound());

            var d2 = new Day23("Day23Test2.txt");
            Assert.AreEqual(110, d2.SpaceAfter10Moves());
            Assert.AreEqual(20, d2.FirstStationaryRound());
        }

        [TestMethod]
        public void Day24()
        {
            var d = new Day24("Day24Test.txt");
            Assert.AreEqual(18, d.TimeToGoal());
            Assert.AreEqual(54, d.TimeForTripleTrip());
        }

        [TestMethod]
        public void Day25()
        {
            var d = new Day25("Day25Test.txt");
            Assert.AreEqual("2=-1=0", d.Part1());
        }
    }
}