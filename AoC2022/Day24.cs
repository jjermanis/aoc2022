using System;
using System.Collections.Generic;

namespace AoC2022
{
    public class Day24 : DayBase, IDay
    {
        // TODO this runs very slow. 24-1 is fast (~200ms), but 24-2 is very slow (~20 seconds).
        // There is no clear reason for it to run so slow.
        // Also, there is some code that can be simplified.
        // Profiler indicates that a lot of time is spent in HashSet.Contains(). Switch tuple
        // keys to ints contain multiple bit-shifted values?
        private enum Direction
        {
            Left,
            Right,
            Down,
            Up
        }
        private class Valley
        {
            private readonly HashSet<(int x, int y, Direction d)> StartBlizzards;
            public readonly HashSet<(int x, int y)> Walls;
            private readonly Dictionary<int, HashSet<(int x, int y, Direction d)>> Blizzards;
            private readonly Dictionary<int, HashSet<(int x, int y)>> BlizzardPictures;
            public readonly int Width;
            public readonly int Height;

            public Valley(IEnumerable<string> lines)
            {
                Width = -1;
                StartBlizzards = new HashSet<(int x, int y, Direction d)>();
                Blizzards = new Dictionary<int, HashSet<(int x, int y, Direction d)>>();
                BlizzardPictures = new Dictionary<int, HashSet<(int x, int y)>>();

                var y = 0;
                foreach (var line in lines)
                {
                    if (Width < 0)
                        Width = line.Length;
                    for (int x = 0; x < line.Length; x++)
                    {
                        var curr = line[x];
                        if (curr != '.' && curr != '#')
                        {
                            var dir = curr switch
                            {
                                '<' => Direction.Left,
                                '>' => Direction.Right,
                                'v' => Direction.Down,
                                '^' => Direction.Up,
                                _ => throw new Exception()
                            };
                            StartBlizzards.Add((x, y, dir));

                        }
                    }
                    y++;
                }
                Height = y;
                Blizzards[0] = StartBlizzards;
                Walls = new HashSet<(int x, int y)>();
                for (int wx = 0; wx < Width; wx++)
                {
                    Walls.Add((wx, -1));
                    Walls.Add((wx, 0));
                    Walls.Add((wx, Height-1));
                }
                Walls.Remove((1, 0));
                Walls.Remove((Width-2,Height-1));
                for (int wy = 0; wy < Height; wy++)
                {
                    Walls.Add((0, wy));
                    Walls.Add((Width-1, wy));
                }
            }

            public HashSet<(int x, int y)> GetBlizzardPicture(int time)
            {
                if (BlizzardPictures.ContainsKey(time))
                    return BlizzardPictures[time];
                HashSet<(int x, int y, Direction d)> nextBlizzard;
                if (Blizzards.ContainsKey(time))
                {
                    nextBlizzard = Blizzards[time];
                }
                else
                {
                    nextBlizzard = new HashSet<(int x, int y, Direction d)>();
                    foreach (var (x, y, d) in Blizzards[time-1])
                    {
                        var (currX, currY) = (x, y);
                        switch (d)
                        {
                            case Direction.Left:
                                currX--;
                                if (currX == 0)
                                    currX = Width - 2;
                                break;
                            case Direction.Right:
                                currX++;
                                if (currX == Width - 1)
                                    currX = 1;
                                break;
                            case Direction.Down:
                                currY++;
                                if (currY == Height - 1)
                                    currY = 1;
                                break;
                            case Direction.Up:
                                currY--;
                                if (currY == 0)
                                    currY = Height - 2;
                                break;
                        }
                        nextBlizzard.Add((currX, currY, d));
                    }
                    Blizzards[time] = nextBlizzard;
                }

                var result = new HashSet<(int x, int y)>();
                foreach(var (x,y,_) in nextBlizzard)
                {
                    if (!result.Contains((x, y)))
                        result.Add((x, y));
                }

                BlizzardPictures[time] = result;
                return result;
            }
        }

        private readonly Valley _valley;

        public Day24(string filename)
            => _valley = new Valley(TextFileLines(filename));

        public Day24() : this("Day24.txt")
        {
        }

        public void Do()
        {
            Console.WriteLine($"{nameof(TimeToGoal)}: {TimeToGoal()}");
            Console.WriteLine($"{nameof(TimeForTripleTrip)}: {TimeForTripleTrip()}");
        }

        public int TimeToGoal()
            => TripTime(0, true);

        public int TimeForTripleTrip()
        {
            var t = TripTime(0, true);
            t = TripTime(t, false);
            return TripTime(t, true);
        }

        private int TripTime(int startTime, bool toGoal)
        {
            int xlr = _valley.Width - 2;
            int ylr = _valley.Height - 1;
            int startX = toGoal ? 1 : xlr;
            int startY = toGoal ? 0 : ylr;
            int endX = !toGoal ? 1 : xlr;
            int endY = !toGoal ? 0 : ylr;

            var attempts = new Queue<(int x, int y, int t)>();
            var visited = new HashSet<(int x, int y, int t)>();
            attempts.Enqueue((startX, startY, startTime));
            while (attempts.Count > 0)
            {
                var (x, y, t) = attempts.Dequeue();

                var nextBlizPic = _valley.GetBlizzardPicture(t + 1);
                var moves = GetMoveCandidates(x, y, nextBlizPic);

                foreach (var (nX, nY) in moves)
                {
                    if (nX == endX && nY == endY)
                        return t + 1;
                    if (!visited.Contains((nX, nY, t + 1)))
                    {
                        visited.Add((nX, nY, t + 1));
                        attempts.Enqueue((nX, nY, t + 1));
                    }
                }
            }
            throw new Exception("No path found");
        }

        private List<(int x, int y)> GetMoveCandidates(
            int currX, int currY,
            HashSet<(int x, int y)> pic)
        {
            var result = new List<(int x, int y)>();
            if (!pic.Contains((currX, currY)))
                result.Add((currX, currY));

            // Check left
            if (!pic.Contains((currX-1, currY)) && !_valley.Walls.Contains((currX - 1, currY)))
                result.Add((currX-1, currY));
            // Check up
            if (!pic.Contains((currX, currY-1)) && !_valley.Walls.Contains((currX, currY-1)))
                result.Add((currX, currY-1));
            // Check right
            if (!pic.Contains((currX+1, currY)) && !_valley.Walls.Contains((currX + 1, currY)))
                result.Add((currX+1, currY));
            // Check down
            if (!pic.Contains((currX, currY+1)) && !_valley.Walls.Contains((currX, currY+1)))
                result.Add((currX, currY+1));

            return result;
        }

    }
}