using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC2022
{
    public class Day19 : DayBase, IDay
    {
        // TODO this is, by far, the slowest, running well over 10 minutes.
        // 16-1 is about 2 minutes.
        // 16-2 is about 7 minutes.

        private class Blueprint
        {
            private const string REGEX_PATTERN =
                @"Blueprint (\d*): Each ore robot costs (\d*) ore. Each clay robot costs (\d*) ore. " +
                @"Each obsidian robot costs (\d*) ore and (\d*) clay. " + 
                @"Each geode robot costs (\d*) ore and (\d*) obsidian.";

            public readonly int Id;
            public readonly int OreBotOreCost;
            public readonly int ClayBotOreCost;
            public readonly int ObsidianBotOreCost;
            public readonly int ObsidianBotClayCost;
            public readonly int GeodeBotOreCost;
            public readonly int GeodeBotObsidianCost;

            public Blueprint(string line)
            {
                var m = Regex.Match(line, REGEX_PATTERN);
                Id = int.Parse(m.Groups[1].Value);
                OreBotOreCost = int.Parse(m.Groups[2].Value);
                ClayBotOreCost = int.Parse(m.Groups[3].Value);
                ObsidianBotOreCost = int.Parse(m.Groups[4].Value);
                ObsidianBotClayCost = int.Parse(m.Groups[5].Value);
                GeodeBotOreCost = int.Parse(m.Groups[6].Value);
                GeodeBotObsidianCost = int.Parse(m.Groups[7].Value);
            }
        }

        private readonly IList<Blueprint> _blueprints;

        public Day19(string filename)
        { 
            var lines = TextFileLines(filename);
            _blueprints = new List<Blueprint>();
            foreach (var line in lines)
                _blueprints.Add(new Blueprint(line));
        }

        public Day19() : this("Day19.txt")
        {
        }

        public void Do()
        {
            Console.WriteLine($"{nameof(GeodeQualitySum)}: {GeodeQualitySum()}");
            //Console.WriteLine($"{nameof(FirstThreeGeodeProduct)}: {FirstThreeGeodeProduct()}");
        }

        public int GeodeQualitySum()
        {
            var result = 0;
            foreach (var bp in _blueprints)
                result += (bp.Id * MaxGeodes(bp, 24));
            return result;
        }

        public int FirstThreeGeodeProduct()
        {
            var result = 1;
            for (int i = 0; i < 3; i++)
                result *= MaxGeodes(_blueprints[i], 32);
            return result;
        }

        private int MaxGeodes(Blueprint bp, int totalTime)
        {
            var maxOreBots = new[] { bp.ClayBotOreCost, bp.ObsidianBotOreCost, bp.GeodeBotOreCost }.Max();
            var maxClayBots = bp.ObsidianBotClayCost;
            var maxObsidianBots = bp.GeodeBotObsidianCost;

            var result = MaxGeodes(bp, totalTime, 
                1, 0, 0, 0,
                0, 0, 0, 0,
                maxOreBots, maxClayBots, maxObsidianBots,
                new int[totalTime+1]);
            Console.WriteLine($"Blueprint {bp.Id}: {result}");
            return result;
        }

        private int MaxGeodes(
            Blueprint bp,
            int timeRemaining,
            int orbBotCount, int clayBotCount, int obsidianBotCount, int geodeBotCount,
            int orbCount, int clayCount, int obsidianCount, int geodeCount,
            int maxOreBots, int maxClayBots, int maxObsidianBots,
            int[] maxGeodes)
        {
            if (timeRemaining <= 0)
                return geodeCount;

            // Check geodes
            if (geodeCount > maxGeodes[timeRemaining])
                maxGeodes[timeRemaining] = geodeCount;
            if (maxGeodes[timeRemaining] > geodeCount + 2)
                return 0;

            // Determine what can be built
            var buildOre = orbCount >= bp.OreBotOreCost && orbBotCount < maxOreBots;
            var buildClay = orbCount >= bp.ClayBotOreCost && clayBotCount < maxClayBots;
            var buildObsidian = orbCount >= bp.ObsidianBotOreCost && clayCount >= bp.ObsidianBotClayCost && obsidianBotCount < maxObsidianBots;
            var buildGeode = orbCount >= bp.GeodeBotOreCost && obsidianCount >= bp.GeodeBotObsidianCost;

            // Update for mining
            orbCount += orbBotCount;
            clayCount += clayBotCount;
            obsidianCount += obsidianBotCount;
            geodeCount += geodeBotCount;

            // Try what's available
            var max = 0;

            if (buildGeode)
                max = Math.Max(max, MaxGeodes(bp, timeRemaining - 1,
                    orbBotCount + 1, clayBotCount, obsidianBotCount, geodeBotCount + 1,
                    orbCount - bp.GeodeBotOreCost, clayCount, obsidianCount - bp.GeodeBotObsidianCost, geodeCount,
                    maxOreBots, maxClayBots, maxObsidianBots, maxGeodes));
            if (buildObsidian)
                max = Math.Max(max, MaxGeodes(bp, timeRemaining - 1,
                    orbBotCount, clayBotCount, obsidianBotCount + 1, geodeBotCount,
                    orbCount - bp.ObsidianBotOreCost, clayCount - bp.ObsidianBotClayCost, obsidianCount, geodeCount,
                    maxOreBots, maxClayBots, maxObsidianBots, maxGeodes));
            if (buildClay)
                max = Math.Max(max, MaxGeodes(bp, timeRemaining - 1,
                    orbBotCount, clayBotCount + 1, obsidianBotCount, geodeBotCount,
                    orbCount - bp.ClayBotOreCost, clayCount, obsidianCount, geodeCount,
                    maxOreBots, maxClayBots, maxObsidianBots, maxGeodes));
            if (buildOre)
                max = Math.Max(max, MaxGeodes(bp, timeRemaining - 1,
                    orbBotCount + 1, clayBotCount, obsidianBotCount, geodeBotCount,
                    orbCount - bp.OreBotOreCost, clayCount, obsidianCount, geodeCount,
                    maxOreBots, maxClayBots, maxObsidianBots, maxGeodes));
            max = Math.Max(max, MaxGeodes(bp, timeRemaining - 1,
                orbBotCount, clayBotCount, obsidianBotCount, geodeBotCount,
                orbCount, clayCount, obsidianCount, geodeCount,
                maxOreBots, maxClayBots, maxObsidianBots, maxGeodes));

            return max;
       
        }
    }
}