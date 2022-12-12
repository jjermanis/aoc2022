using System;
using System.Collections.Generic;

namespace AoC2022
{
    public class Day09 : DayBase, IDay
    {
        private readonly IDictionary<char, (int x, int y)> _VECTORS =
            new Dictionary<char, (int x, int y)>
            {
                { 'R', (1, 0) },
                { 'L', (-1, 0) },
                { 'D', (0, 1) },
                { 'U', (0, -1) },
            };

        private readonly IEnumerable<string> _lines;

        public Day09(string filename)
            => _lines = TextFileLines(filename);

        public Day09() : this("Day09.txt")
        {
        }

        public void Do()
        {
            Console.WriteLine($"{nameof(UniqueTailPositions2)}: {UniqueTailPositions2()}");
            Console.WriteLine($"{nameof(UniqueTailPositions10)}: {UniqueTailPositions10()}");
        }

        public int UniqueTailPositions2()
            => UniqueTailPositions(2);

        public int UniqueTailPositions10()
            => UniqueTailPositions(10);

        private int UniqueTailPositions(int ropeLength)
        {
            var uniqueTailLocs = new HashSet<(int x, int y)>();
            var knotPositions = new List<(int x, int y)>(ropeLength);

            for (var i = 0; i < ropeLength; i++)
                knotPositions.Add((0, 0));

            foreach (var move in _lines)
            {
                var vector = _VECTORS[move[0]];
                var distance = int.Parse(move[1..]);

                for (var i = 0; i < distance; i++)
                {
                    var front = knotPositions[0];
                    front.x += vector.x;
                    front.y += vector.y;
                    knotPositions[0] = front;

                    for (var knot = 1; knot < ropeLength; knot++)
                    {
                        var back = knotPositions[knot];
                        back = UpdateNext(front.x, front.y, back.x, back.y);
                        knotPositions[knot] = back;
                        front = back;
                    }

                    var tail = knotPositions[ropeLength - 1];
                    if (!uniqueTailLocs.Contains((tail.x, tail.y)))
                        uniqueTailLocs.Add((tail.x, tail.y));
                }
            }

            return uniqueTailLocs.Count;
        }

        private (int, int) UpdateNext(
            int frontX, int frontY,
            int backX, int backY)
        {
            var resultX = backX;
            var resultY = backY;

            var deltaX = frontX - backX;
            var deltaY = frontY - backY;

            if (Math.Abs(deltaX * deltaY) >= 2)
            {
                resultX += deltaX / Math.Abs(deltaX);
                resultY += deltaY / Math.Abs(deltaY);
            }
            else
            {
                if (Math.Abs(deltaX) == 2)
                    resultX += deltaX / Math.Abs(deltaX);
                else if (Math.Abs(deltaY) == 2)
                    resultY += deltaY / Math.Abs(deltaY);
            }

            return (resultX, resultY);
        }
    }
}