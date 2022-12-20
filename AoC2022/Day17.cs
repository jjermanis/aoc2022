using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC2022
{
    public class Day17 : DayBase, IDay
    {
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

        public Day17(string filename)
            => _winds = TextFile(filename);

        public Day17() : this("Day17.txt")
        {
        }

        public void Do()
        {
            Console.WriteLine($"{nameof(Part1)}: {Part1()}");
            Console.WriteLine($"{nameof(Part2)}: {Part2()}");
        }

        public long Part1()
            => TowerHeight(2022);

        public int Part2()
        {
            return 0;
        }

        private long TowerHeight(int rockCount)
        {
            var height = 0;
            var tower = new List<char[]>();
            var time = -1;
            int winds_mod = _winds.Length;

            for (var c=0; c < rockCount; c++)
            {
                for (int i=tower.Count; i<height+7; i++)
                    tower.Add(new char[7]);

                int x = 2;
                int y = height + 3;
                var shape = ROCKS[c % 5];
                // Tweak for the plus rock
                if (c % 5 == 1)
                    y++;

                while (true)
                {
                    time++;
                    // Try movement due to wind
                    var windDelta = _winds[time % winds_mod] switch
                    {
                        '>' => 1,
                        '<' => -1,
                        _ => throw new Exception()
                    };
                    x += windDelta;
                    var hasWindSpace = true;
                    foreach (var spot in shape)
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
                    foreach (var spot in shape)
                    {
                        if (y + spot.y < 0)
                            hasFallSpace = false;
                        else if (tower[y + spot.y][x + spot.x] == '#')
                            hasFallSpace = false;
                    }
                    if (!hasFallSpace)
                    {
                        y++;
                        foreach (var spot in shape)
                        {
                            tower[y + spot.y][x + spot.x] = '#';
                        }
                        break;
                    }
                }

                // Figure out new height
                var tempHeight = height + 3;
                while (true)
                {
                    var foundTop = false;
                    foreach (char v in tower[tempHeight])
                        if (v == '#')
                            foundTop = true;
                    if (foundTop)
                        break;
                    tempHeight--;
                }
                height = tempHeight + 1;
            }
            return height;
        }

    }
}