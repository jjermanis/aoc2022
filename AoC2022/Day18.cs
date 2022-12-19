using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC2022
{
    public class Day18 : DayBase, IDay
    {
        // TODO compiler warnings regarding tuples. Fix it.

        private readonly List<(int dx, int dy, int dz)> NEIGHBORS = new List<(int dx, int dy, int dz)>() 
        {
            (-1, 0, 0),
            (1, 0, 0),
            (0, -1, 0),
            (0, 1, 0),
            (0, 0, -1),
            (0, 0, 1),
        };

        private readonly IEnumerable<string> _lines;

        public Day18(string filename)
            => _lines = TextFileLines(filename);

        public Day18() : this("Day18.txt")
        {
        }

        public void Do()
        {
            Console.WriteLine($"{nameof(OpenFaceCount)}: {OpenFaceCount()}");
            Console.WriteLine($"{nameof(ExposedFaceCount)}: {ExposedFaceCount()}");
        }

        public int OpenFaceCount()
        {
            var cubes = ParseCubes();

            int result = 0;
            // Look at all the cubes...
            foreach ((int x, int y, int z) c in cubes)
            {
                // Look at the six faces. If there is nothing there, it is open
                foreach ((int dx, int dy, int dz) n in NEIGHBORS)
                {
                    var curr = (c.x + n.dx, c.y + n.dy, c.z + n.dz);
                    if (!cubes.Contains(curr))
                        result++;
                }
            }
            return result;
        }

        public int ExposedFaceCount()
        {
            var cubes = ParseCubes();
            var water = FillWater(cubes);

            int result = 0;
            // Look at all the cubes...
            foreach ((int x, int y, int z) c in cubes)
            {
                // Look at the six faces. If there is water there, it is exposed
                foreach ((int dx, int dy, int dz) n in NEIGHBORS)
                {
                    var curr = (c.x + n.dx, c.y + n.dy, c.z + n.dz);
                    if (water.Contains(curr))
                        result++;
                }
            }
            return result;
        }

        private HashSet<(int x, int y, int z)> ParseCubes()
        {
            var result = new HashSet<(int x, int y, int z)>();

            foreach(var line in _lines)
            {
                var c = line.Split(',').Select(c => int.Parse(c)).ToArray();
                result.Add((c[0], c[1], c[2]));
            }
            return result;
        }

        private HashSet<(int x, int y, int z)> FillWater(HashSet<(int x, int y, int z)> cubes)
        {
            var result = new HashSet<(int x, int y, int z)>();
            var borders = GetBorders(cubes);
            var flow = new Queue<(int x, int y, int z)>();
            flow.Enqueue((borders.minX, borders.minY, borders.minZ));
            while (flow.Count > 0)
            {
                var curr = flow.Dequeue();
                if (curr.x >= borders.minX && curr.x <= borders.maxX &&
                    curr.y >= borders.minY && curr.y <= borders.maxY &&
                    curr.z >= borders.minZ && curr.z <= borders.maxZ &&
                    !result.Contains(curr) && !cubes.Contains(curr))
                {
                    result.Add(curr);
                    foreach (var n in NEIGHBORS)
                    {
                        var nn = (curr.x + n.dx, curr.y + n.dy, curr.z + n.dz);
                        flow.Enqueue(nn);
                    }
                }
            }
            return result;

        }

        private (int minX, int minY, int minZ, int maxX, int maxY, int maxZ) GetBorders(
            HashSet<(int x, int y, int z)> cubes)
        {
            var minX = int.MaxValue;
            var minY = int.MaxValue;
            var minZ = int.MaxValue;
            var maxX = int.MinValue;
            var maxY = int.MinValue;
            var maxZ = int.MinValue;

            foreach (var cube in cubes)
            {
                if (cube.x < minX) minX = cube.x;
                if (cube.y < minY) minY = cube.y;
                if (cube.z < minZ) minZ = cube.z;
                if (cube.x > maxX) maxX = cube.x;
                if (cube.y > maxY) maxY = cube.y;
                if (cube.z > maxZ) maxZ = cube.z;
            }

            return (minX-1, minY-1, minZ-1, maxX+1, maxY+1, maxZ+1);
        }
    }
}