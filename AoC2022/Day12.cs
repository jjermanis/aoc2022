using System;
using System.Collections.Generic;

namespace AoC2022
{
    public class Day12 : DayBase, IDay
    {
        private readonly IEnumerable<string> _lines;

        public Day12(string filename)
            => _lines = TextFileLines(filename);

        public Day12() : this("Day12.txt")
        {
        }

        public void Do()
        {
            Console.WriteLine($"{nameof(PathLengthFromStart)}: {PathLengthFromStart()}");
            Console.WriteLine($"{nameof(PathLengthFromLowPoint)}: {PathLengthFromLowPoint()}");
        }

        public int PathLengthFromStart()
            => FindDistance(false);

        public int PathLengthFromLowPoint()
            // TODO - for this case, instead of "going back" when a's are found, would
            // it make sense to just start from every a?
            => FindDistance(true);

        private int FindDistance(bool resetOnA)
        {
            var result = int.MaxValue;
            (var map, var start, var end) = InitMap();
            var distances = new Dictionary<(int x, int y), int>();
            var queue = new Queue<(int x, int y, int distance)>();
            queue.Enqueue((start.x, start.y, 0));
            while (queue.Count > 0)
            {
                var curr = queue.Dequeue();
                if (curr.x == end.x && curr.y == end.y)
                    result = Math.Min(result, curr.distance);

                if (!distances.ContainsKey((curr.x, curr.y)) ||
                    curr.distance < distances[(curr.x, curr.y)] )
                {
                    distances[(curr.x, curr.y)] = curr.distance;

                    AddMoveIfPossible(map, distances, queue, curr, (0, 1), resetOnA);
                    AddMoveIfPossible(map, distances, queue, curr, (0, -1), resetOnA);
                    AddMoveIfPossible(map, distances, queue, curr, (1, 0), resetOnA);
                    AddMoveIfPossible(map, distances, queue, curr, (-1, 0), resetOnA);
                }
            }
            return result;
        }

        private (IDictionary<(int x, int y), char> map, (int x, int y) start, (int x, int y) end) InitMap()
        {
            var start = (-1, -1);
            var end = (-1, -1);
            var map = new Dictionary<(int x, int y), char>();
            var y = -1;
            foreach(var line in _lines)
            {
                y++;
                for (int x = 0; x < line.Length; x++)
                {
                    char c = line[x];
                    if (c == 'S')
                    {
                        start = (x, y);
                        c = 'a';
                    }
                    else if (c == 'E')
                    {
                        end = (x, y);
                        c = 'z';
                    }
                    map[(x, y)] = c;
                }
            }
            return (map, start, end);
        }

        private void AddMoveIfPossible(
            IDictionary<(int x, int y), char> map,
            IDictionary<(int x, int y), int> distances,
            Queue<(int x, int y, int distance)> queue,
            (int x, int y, int distance) curr,
            (int x, int y) delta,
            bool resetOnA)
        {
            var nextX = curr.x + delta.x;
            var nextY = curr.y + delta.y;
            var nextD = curr.distance + 1;

            if (!map.ContainsKey((nextX, nextY)))
                return;
            if (!resetOnA && distances.ContainsKey((nextX, nextY)))
                return;
            if (resetOnA && distances.ContainsKey((nextX, nextY)) && distances[((nextX, nextY))] <= nextD)
                return;
            var currAlt = map[(curr.x, curr.y)];
            var nextAlt = map[(nextX, nextY)];
            if (nextAlt - currAlt > 1)
                return;
            if (resetOnA && nextAlt == 'a')
                nextD = 0;
            queue.Enqueue((nextX, nextY, nextD));
            return;
        }
    }
}