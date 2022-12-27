using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC2022
{
    public class Day22 : DayBase, IDay
    {
        private readonly List<(int x, int y)> VECTORS = new List<(int, int)>()
        {
            (1, 0), (0, 1), (-1, 0), (0, -1)
        };

        private readonly IList<string> _lines;

        public Day22(string filename)
            => _lines = TextFileStringList(filename);

        public Day22() : this("Day22.txt")
        {
        }

        public void Do()
        {
            Console.WriteLine($"{nameof(Password2D)}: {Password2D()}");
            Console.WriteLine($"{nameof(Part2)}: {Part2()}");
        }

        public int Password2D()
            => NavigateMap(Try2DWraparound);

        public int Part2()
        {
            return 0;
        }

        private int NavigateMap(
            Func<IDictionary<(int x, int y), char>, int, int, int, (int, int, int)> TryWrapAroundFunc)
        {
            (var start, var map, var path) = Parse();
            var currX = start.x;
            var currY = start.y;
            var currDir = 0;

            foreach (var move in path)
            {
                if (!char.IsDigit(move[0]))
                {
                    currDir = move[0] switch
                    {
                        'L' => currDir - 1,
                        'R' => currDir + 1,
                        _ => throw new Exception("Unknown direction")
                    };
                    currDir = (currDir + 4) % 4;
                }
                else
                {
                    var distance = int.Parse(move);
                    var vector = VECTORS[currDir];
                    for (int i = 0; i < distance; i++)
                    {
                        (var nextX, var nextY) = (currX + vector.x, currY + vector.y);

                        if (map.ContainsKey((nextX, nextY)))
                        {
                            if (map[(nextX, nextY)] == '.')
                            {
                                (currX, currY) = (nextX, nextY);
                            }
                        }
                        else
                        {
                            (currX, currY, currDir) = TryWrapAroundFunc(map, currX, currY, currDir);
                        }
                    }
                }
            }
            return currY * 1000 + currX * 4 + currDir;
        }

        private (int x, int y, int direction) Try2DWraparound(
            IDictionary<(int x, int y), char> map,
            int x, int y, int dir)
        {
            (var wrapX, var wrapY) = (x, y);
            var vector = VECTORS[dir];

            while (true)
            {
                (wrapX, wrapY) = (wrapX - vector.x, wrapY - vector.y);
                if (!map.ContainsKey((wrapX, wrapY)))
                {
                    (wrapX, wrapY) = (wrapX + vector.x, wrapY + vector.y);
                    break;
                }
            }
            if (map[(wrapX, wrapY)] == '.')
                return (wrapX, wrapY, dir);

            return (x, y, dir);
        }

        private ((int x, int y) start, IDictionary<(int x, int y), char> map, IList<string> path) Parse()
        {
            var map = new Dictionary<(int x, int y), char>();
            var startY = 1;
            var startX = -1;

            for (int y=0; y < _lines.Count-2; y++)
            {
                var line = _lines[y];
                for (int x = 0; x < line.Length; x++)
                {
                    var curr = line[x];
                    if (curr != ' ')
                    {
                        map[(x+1, y+1)] = curr;
                        if (startX < 0 && map[(x+1, y+1)] == '.')
                            startX = x;
                    }
                }
            }
            
            var rawPath = _lines[_lines.Count-1];
            var path = new List<string>();
            var currDistance = 0;
            for (int x = 0; x < rawPath.Length; x++)
            {
                var curr = rawPath[x];

                if (char.IsDigit(curr))
                {
                    currDistance *= 10;
                    currDistance += curr - '0';
                }
                else
                {
                    if (currDistance > 0)
                    {
                        path.Add(currDistance.ToString());
                        currDistance = 0;
                    }
                    path.Add(curr.ToString());
                }
            }
            if (currDistance > 0)
                path.Add(currDistance.ToString());

            return ((startX, startY), map, path);
        }
    }
}