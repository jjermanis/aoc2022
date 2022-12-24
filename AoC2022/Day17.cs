using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC2022
{
    public class Day17 : DayBase, IDay
    {

        // TODO this finds the problem for _my_ data. But requires some testing up
        // front, doesn't work on the sample. Clean up would help.

        private static readonly List<(int x, int y)> MINUS_ROCK = 
            new List<(int x, int y)>() { (0, 0), (1, 0), (2, 0), (3, 0) };
        private static readonly List<(int x, int y)> PLUS_ROCK =
            new List<(int x, int y)>() { (0, 0), (1, -1), (1, 0), (1, 1), (2, 0) };
        private static readonly List<(int x, int y)> RIGHT_ROCK =
            new List<(int x, int y)>() { (0, 0), (1, 0), (2, 0), (2, 1), (2, 2) };
        private static readonly List<(int x, int y)> PIPE_ROCK =
            new List<(int x, int y)>() { (0, 0), (0, 1), (0, 2), (0, 3) };
        private static readonly List<(int x, int y)> CUBE_ROCK =
            new List<(int x, int y)>() { (0, 0), (0, 1), (1, 1), (1, 0) };

        private static readonly List<List<(int x, int y)>> ROCKS =
            new List<List<(int x, int y)>>() 
            { MINUS_ROCK, PLUS_ROCK, RIGHT_ROCK, PIPE_ROCK, CUBE_ROCK };

        private readonly string _winds;
        private readonly int _windsLen;

        public Day17(string filename)
        {
            _winds = TextFile(filename);
            _windsLen = _winds.Length;
        }

        public Day17() : this("Day17.txt")
        {
        }

        public void Do()
        {
            Console.WriteLine($"{nameof(SmallTowerHeight)}: {SmallTowerHeight()}");
            Console.WriteLine($"{nameof(MassiveTowerHeight)}: {MassiveTowerHeight()}");
        }

        public long SmallTowerHeight()
            => TowerHeight(2022, false);

        public long MassiveTowerHeight()
            => TowerHeight(1000000000000, true);

        private long TowerHeight(long rockCount, bool tryShortcut)
        {
            var height = 0;
            var tower = new List<char[]>();
            var time = 0;
            //var cache = new Dictionary<(int windTime, int shape), int>();
            var firstPatternHeight = -1;
            var firstPatternRockCount = -1;
            long bonus = 0;
            for (long c=0; c < rockCount; c++)
            {
                for (int i=tower.Count; i<height+8; i++)
                    tower.Add(new char[7]);

                var rock = ROCKS[(int)(c % 5)];
                int y = height + 3;
                // Tweak for the PLUS_ROCK
                if (c % 5 == 1)
                    y++;

                time = DropRock(rock, tower, 2, y, time);

                // Update height
                height = TowerTop(tower, height + 3);

                if (tryShortcut && height > 500)
                {
                    if (c % 5 == 0 && time % _windsLen == 450)
                    {
                        if (firstPatternHeight == -1 )
                        {
                            firstPatternHeight = height;
                            firstPatternRockCount = (int)c; // Should this be c+1?
                        }
                        else
                        {
                            var deltaHeight = height - firstPatternHeight;
                            var deltaRockCount = (int)c - firstPatternRockCount;

                            var fakeRounds = (rockCount - c) / deltaRockCount;
                            bonus = fakeRounds * deltaHeight;
                            c += fakeRounds * deltaRockCount;
                        }
                    }
                    /*
                    if (cache.ContainsKey((time % _windsLen, c % 5)))
                        height = height;
                    cache[(time % _windsLen, c % 5)] = height;
                    */
                }
            }
            return height + bonus;
        }

        private int DropRock(
            List<(int x, int y)> rock,
            List<char[]> tower,
            int locX, int locY,
            int windTime)
        {
            var time = windTime;
            var x = locX;
            var y = locY;

            while (true)
            {
                // Try movement due to wind
                var windDelta = _winds[time % _windsLen] switch
                {
                    '>' => 1,
                    '<' => -1,
                    _ => throw new Exception()
                };
                x += windDelta;
                var hasWindSpace = true;
                foreach (var spot in rock)
                {
                    if (x + spot.x < 0 || x + spot.x > 6)
                        hasWindSpace = false;
                    else if (tower[y + spot.y][x + spot.x] == '#')
                        hasWindSpace = false;
                }
                if (!hasWindSpace)
                    x -= windDelta;

                // Try movement due to falling
                y--;
                var hasFallSpace = true;
                foreach (var spot in rock)
                {
                    if (y + spot.y < 0)
                        hasFallSpace = false;
                    else if (tower[y + spot.y][x + spot.x] == '#')
                        hasFallSpace = false;
                }
                if (!hasFallSpace)
                {
                    y++;
                    foreach (var spot in rock)
                    {
                        tower[y + spot.y][x + spot.x] = '#';
                    }
                    return ++time;
                }
                time++;
            }
        }

        private int TowerTop(
            List<char[]> tower,
            int maxPossible)
        {
            var currRow = maxPossible;
            while (true)
            {
                foreach (char x in tower[currRow])
                    if (x == '#')
                        return currRow + 1;
                currRow--;
            }
        }
    }
}