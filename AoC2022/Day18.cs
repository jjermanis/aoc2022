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
            foreach (var (cx, cy, cz) in cubes)
            {
                // Look at the six faces. If there is nothing there, it is open
                foreach (var (nx, ny, nz) in NEIGHBORS)
                {
                    var curr = (cx + nx, cy + ny, cz + nz);
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
            foreach (var (cx, cy, cz) in cubes)
            {
                // Look at the six faces. If there is water there, it is exposed
                foreach (var (nx, ny, nz) in NEIGHBORS)
                {
                    var curr = (cx + nx, cy + ny, cz + nz);
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
            var (minX, minY, minZ, maxX, maxY, maxZ) = GetBorders(cubes);
            var flow = new Queue<(int x, int y, int z)>();
            flow.Enqueue((minX, minY, minZ));
            while (flow.Count > 0)
            {
                var curr = flow.Dequeue();
                if (curr.x >= minX && curr.x <= maxX &&
                    curr.y >= minY && curr.y <= maxY &&
                    curr.z >= minZ && curr.z <= maxZ &&
                    !result.Contains(curr) && !cubes.Contains(curr))
                {
                    result.Add(curr);
                    foreach (var (nx, ny, nz) in NEIGHBORS)
                    {
                        var nn = (curr.x + nx, curr.y + ny, curr.z + nz);
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

            foreach (var (x,y,z) in cubes)
            {
                if (x < minX) minX = x;
                if (y < minY) minY = y;
                if (z < minZ) minZ = z;
                if (x > maxX) maxX = x;
                if (y > maxY) maxY = y;
                if (z > maxZ) maxZ = z;
            }

            return (minX-1, minY-1, minZ-1, maxX+1, maxY+1, maxZ+1);
        }
    }
}