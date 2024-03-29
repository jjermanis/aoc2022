﻿using System;
using System.Collections.Generic;

namespace AoC2022
{
    public class Day14 : DayBase, IDay
    {

        private struct Point
        {
            public readonly int X;
            public readonly int Y;

            public Point(int x, int y)
            {
                X = x;
                Y = y;
            }
            public Point(string rawCoordinates)
            {
                var coords = rawCoordinates.Split(',');
                X = int.Parse(coords[0]);
                Y = int.Parse(coords[1]);
            }
            public override string ToString()
            {
                return $"{X},{Y}";
            }
            public IEnumerable<Point> PointChain(Point dest)
            {
                (var deltaX, var deltaY) = (dest.X - X, dest.Y - Y);
                var vectorX = 0;
                if (deltaX != 0)
                    vectorX = deltaX / Math.Abs(deltaX);
                var vectorY = 0;
                if (deltaY != 0)
                    vectorY = deltaY / Math.Abs(deltaY);
                (var currX, var currY) = (X, Y);
                yield return new Point(currX, currY);
                while (currX != dest.X || currY != dest.Y)
                {
                    currX += vectorX;
                    currY += vectorY;
                    yield return new Point(currX, currY);
                }
            }
        }

        private readonly IEnumerable<string> _lines;

        public Day14(string filename)
            => _lines = TextFileLines(filename);

        public Day14() : this("Day14.txt")
        {
        }

        public void Do()
        {
            Console.WriteLine($"{nameof(SoundCountToFloor)}: {SoundCountToFloor()}");
            Console.WriteLine($"{nameof(SandCountToSource)}: {SandCountToSource()}");
        }

        public int SoundCountToFloor()
            => SandCountToFill(false);

        public int SandCountToSource()
            => SandCountToFill(true);

        private int SandCountToFill(bool createGround)
        {
            var map = InitMap(createGround);
            int count = 0;
            while (true)
            {
                (var point, var isLanded) = DropSand(map);
                if (!isLanded)
                    break;
                map[(point.X, point.Y)] = 'o';
                count++;
                if (point.Y == 0)
                    break;
            }
            return count;
        }

        private (Point point, bool isLanded) DropSand(IDictionary<(int x, int y), char> map)
        {
            var x = 500;
            var y = 0;

            while (y < 200)
            {
                if (!map.ContainsKey((x, y+1)))
                {
                    y++;
                    continue;
                }
                if (!map.ContainsKey((x-1, y+1)))
                {
                    x--;
                    y++;
                    continue;
                }
                if (!map.ContainsKey((x + 1, y+1)))
                {
                    x++;
                    y++;
                    continue;
                }
                return (new Point(x, y), true);
            }
            return (new Point(-1, -1), false);
        }

        private IDictionary<(int x, int y), char> InitMap(bool createGround)
        {
            var result = new Dictionary<(int x, int y), char>();
            foreach (var line in _lines)
                AddLine(result, line);

            if (createGround)
            {
                var minX = 500;
                var maxX = 500;
                var maxY = 0;

                foreach (var point in result.Keys)
                {
                    minX = Math.Min(minX, point.x);
                    maxX = Math.Max(maxX, point.x);
                    maxY = Math.Max(maxY, point.y);
                }
                minX -= 200;
                maxX += 200;
                maxY += 2;
                AddLine(result, $"{minX},{maxY} -> {maxX},{maxY}");
            }
            return result;
        }

        private void AddLine(
            IDictionary<(int x, int y), char> map, 
            string line)
        {
            var points = line.Split("->");
            var curr = new Point(points[0]);
            for (var i=1; i<points.Length; i++)
            {
                var next = new Point(points[i]);
                foreach (var point in curr.PointChain(next))
                    if (!map.ContainsKey((point.X, point.Y)))
                        map[(point.X, point.Y)] = '#';
                curr = next;
            }
        }
    }
}