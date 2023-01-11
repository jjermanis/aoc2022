using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC2022
{
    public class Day19 : DayBase, IDay
    {
        // TODO a very nice optimization was added, but code is verbose. Clean up.

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
            Console.WriteLine($"{nameof(FirstThreeGeodeProduct)}: {FirstThreeGeodeProduct()}");
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

            var max = geodeCount + (timeRemaining * geodeBotCount);

            if (obsidianBotCount > 0)
            {
                var minutesPassed = 1;
                var tempOrbCount = orbCount;
                var tempClayCount = clayCount;
                var tempObsidianCount = obsidianCount;
                var tempGeodeCount = geodeCount;

                while (tempOrbCount < bp.GeodeBotOreCost || tempObsidianCount < bp.GeodeBotObsidianCost)
                {
                    tempOrbCount += orbBotCount;
                    tempClayCount += clayBotCount;
                    tempObsidianCount += obsidianBotCount;
                    tempGeodeCount += geodeBotCount;
                    minutesPassed++;
                }
                tempOrbCount += orbBotCount;
                tempClayCount += clayBotCount;
                tempObsidianCount += obsidianBotCount;
                tempGeodeCount += geodeBotCount;

                if (timeRemaining - minutesPassed >= 1)
                    max = Math.Max(max, MaxGeodes(bp, timeRemaining - minutesPassed,
                        orbBotCount, clayBotCount, obsidianBotCount, geodeBotCount + 1,
                        tempOrbCount - bp.GeodeBotOreCost, tempClayCount, tempObsidianCount - bp.GeodeBotObsidianCost, tempGeodeCount,
                        maxOreBots, maxClayBots, maxObsidianBots, maxGeodes));
            }
            if (obsidianBotCount < maxObsidianBots && clayBotCount > 0)
            {
                var minutesPassed = 1;
                var tempOrbCount = orbCount;
                var tempClayCount = clayCount;
                var tempObsidianCount = obsidianCount;
                var tempGeodeCount = geodeCount;

                while (tempOrbCount < bp.ObsidianBotOreCost || tempClayCount < bp.ObsidianBotClayCost)
                {
                    tempOrbCount += orbBotCount;
                    tempClayCount += clayBotCount;
                    tempObsidianCount += obsidianBotCount;
                    tempGeodeCount += geodeBotCount;
                    minutesPassed++;
                }
                tempOrbCount += orbBotCount;
                tempClayCount += clayBotCount;
                tempObsidianCount += obsidianBotCount;
                tempGeodeCount += geodeBotCount;

                if (timeRemaining - minutesPassed >= 1)
                    max = Math.Max(max, MaxGeodes(bp, timeRemaining - minutesPassed,
                        orbBotCount, clayBotCount, obsidianBotCount + 1, geodeBotCount,
                        tempOrbCount - bp.ObsidianBotOreCost, tempClayCount - bp.ObsidianBotClayCost, tempObsidianCount, tempGeodeCount,
                        maxOreBots, maxClayBots, maxObsidianBots, maxGeodes));
            }
            if (orbBotCount < maxOreBots)
            {
                var minutesPassed = 1;
                var tempOrbCount = orbCount;
                var tempClayCount = clayCount;
                var tempObsidianCount = obsidianCount;
                var tempGeodeCount = geodeCount;

                while (tempOrbCount < bp.OreBotOreCost)
                {
                    tempOrbCount += orbBotCount;
                    tempClayCount += clayBotCount;
                    tempObsidianCount += obsidianBotCount;
                    tempGeodeCount += geodeBotCount;
                    minutesPassed++;
                }
                tempOrbCount += orbBotCount;
                tempClayCount += clayBotCount;
                tempObsidianCount += obsidianBotCount;
                tempGeodeCount += geodeBotCount;

                if (timeRemaining - minutesPassed >= 1)
                    max = Math.Max(max, MaxGeodes(bp, timeRemaining - minutesPassed,
                        orbBotCount + 1, clayBotCount, obsidianBotCount, geodeBotCount,
                        tempOrbCount - bp.OreBotOreCost, tempClayCount, tempObsidianCount, tempGeodeCount,
                        maxOreBots, maxClayBots, maxObsidianBots, maxGeodes));
            }
            if (clayBotCount < maxClayBots)
            {
                var minutesPassed = 1;
                var tempOrbCount = orbCount;
                var tempClayCount = clayCount;
                var tempObsidianCount = obsidianCount;
                var tempGeodeCount = geodeCount;

                while (tempOrbCount < bp.ClayBotOreCost)
                {
                    tempOrbCount += orbBotCount;
                    tempClayCount += clayBotCount;
                    tempObsidianCount += obsidianBotCount;
                    tempGeodeCount += geodeBotCount;
                    minutesPassed++;
                }
                tempOrbCount += orbBotCount;
                tempClayCount += clayBotCount;
                tempObsidianCount += obsidianBotCount;
                tempGeodeCount += geodeBotCount;

                if (timeRemaining - minutesPassed >= 1)
                    max = Math.Max(max, MaxGeodes(bp, timeRemaining - minutesPassed,
                        orbBotCount, clayBotCount + 1, obsidianBotCount, geodeBotCount,
                        tempOrbCount - bp.ClayBotOreCost, tempClayCount, tempObsidianCount, tempGeodeCount,
                        maxOreBots, maxClayBots, maxObsidianBots, maxGeodes));
            }

            return max;

        }
    }
}