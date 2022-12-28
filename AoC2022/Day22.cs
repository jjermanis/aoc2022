using System;
using System.Collections.Generic;

namespace AoC2022
{
    public class Day22 : DayBase, IDay
    {
        // TODO - this does not work on the sample. The "3D folding" is different and will
        // require different code for Try3DWraparound()
        // Of all the days... this one rates high for code clean up.

        private class MapState
        {
            public int X;
            public int Y;
            public int Direction;
            public readonly int FaceSize;
            public readonly IDictionary<(int x, int y), char> Map;
            public IList<string> Path;

            public MapState(
                int x, int y, 
                IDictionary<(int x, int y), char> map, 
                IList<string> path)
            {
                X = x;
                Y = y;
                Direction = 0;
                FaceSize = X;
                Map = map;
                Path = path;
            }
        }

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
            Console.WriteLine($"{nameof(Password3D)}: {Password3D()}");
        }

        public int Password2D()
            => NavigateMap(Try2DWraparound);

        public int Password3D()
            => NavigateMap(Try3DWraparound);

        private int NavigateMap(
            Action<MapState> TryWrapAroundFunc)
        {
            var mapState = Parse();

            foreach (var move in mapState.Path)
            {
                if (!char.IsDigit(move[0]))
                {
                    var currDir = move[0] switch
                    {
                        'L' => mapState.Direction - 1,
                        'R' => mapState.Direction + 1,
                        _ => throw new Exception("Unknown direction")
                    };
                    mapState.Direction = (currDir + 4) % 4;
                }
                else
                {
                    var distance = int.Parse(move);
                    for (int i = 0; i < distance; i++)
                    {
                        var vector = VECTORS[mapState.Direction];
                        (var nextX, var nextY) = (mapState.X + vector.x, mapState.Y + vector.y);

                        if (mapState.Map.ContainsKey((nextX, nextY)))
                        {
                            if (mapState.Map[(nextX, nextY)] == '.')
                            {
                                (mapState.X, mapState.Y) = (nextX, nextY);
                            }
                        }
                        else
                        {
                            var x = mapState.X;
                            var y = mapState.Y;
                            TryWrapAroundFunc(mapState);
                            if (mapState.X == x && mapState.Y == y)
                                // blocked
                                i = distance;
                        }
                    }
                }
            }
            return (mapState.Y+1) * 1000 + (mapState.X+1) * 4 + mapState.Direction;
        }

        private void Try2DWraparound(MapState mapState)
        {
            (var wrapX, var wrapY) = (mapState.X, mapState.Y);
            var vector = VECTORS[mapState.Direction];

            while (true)
            {
                (wrapX, wrapY) = (wrapX - vector.x, wrapY - vector.y);
                if (!mapState.Map.ContainsKey((wrapX, wrapY)))
                {
                    (wrapX, wrapY) = (wrapX + vector.x, wrapY + vector.y);
                    break;
                }
            }
            if (mapState.Map[(wrapX, wrapY)] == '.')
            {
                mapState.X = wrapX;
                mapState.Y = wrapY;
            }
        }

        private void Try3DWraparound(MapState mapState)
        {
            int currX = mapState.X;
            int currY = mapState.Y;
            var currCube = CubeNum(currX, currY, mapState.FaceSize);
            var option = $"{currCube},{mapState.Direction}";
            int newX;
            int newY;
            int newDir;
            int expectedDest;
            switch (option)
            {
                case "1,2": // 1 to 6
                    newX = 0;
                    newY = (49 - currY) + 100;
                    newDir = 0;
                    expectedDest = 6;
                    break;
                case "1,3": // 1 to 9
                    newX = 0;
                    newY = currX + 100;
                    newDir = 0;
                    expectedDest = 9;
                    break;
                case "2,0": // 2 to 7
                    newX = 99;
                    newY = (49 - currY) + 100;
                    newDir = 2;
                    expectedDest = 7;
                    break;
                case "2,1": // 2 to 4
                    newX = 99;
                    newY = currX - 50;
                    newDir = 2;
                    expectedDest = 4;
                    break;
                case "2,3": // 2 to 9
                    newX = currX-100;
                    newY = 199;
                    newDir = 3;
                    expectedDest = 9;
                    break;
                case "4,0": // 4 to 2
                    newX = currY + 50;
                    newY = 49;
                    newDir = 3;
                    expectedDest = 2;
                    break;
                case "4,2": // 4 to 6
                    newX = currY-50;
                    newY = 100;
                    newDir = 1;
                    expectedDest = 6;
                    break;
                case "6,2": // 6 to 1
                    newX = 50;
                    newY = (149 - currY);
                    newDir = 0;
                    expectedDest = 1;
                    break;
                case "6,3": // 6 to 4
                    newX = 50;
                    newY = currX+50;
                    newDir = 0;
                    expectedDest = 4;
                    break;
                case "7,0": // 7 to 2
                    newX = 149;
                    newY = (149-currY);
                    newDir = 2;
                    expectedDest = 2;
                    break;
                case "7,1": // 7 to 9
                    newX = 49;
                    newY = currX+100;
                    newDir = 2;
                    expectedDest = 9;
                    break;
                case "9,0": // 9 to 7
                    newX = currY-100;
                    newY = 149;
                    newDir = 3;
                    expectedDest = 7;
                    break;
                case "9,1": // 9 to 2
                    newX = currX + 100;
                    newY = 0;
                    newDir = 1;
                    expectedDest = 2;
                    break;
                case "9,2": // 9 to 1
                    newX = currY - 100;
                    newY = 0;
                    newDir = 1;
                    expectedDest = 1;
                    break;
                default:
                    throw new Exception($"Unhandled case: {option}");
            }
            if (CubeNum(newX, newY, mapState.FaceSize) != expectedDest)
                throw new Exception("Wrong face dest");

            if (mapState.Map[(newX, newY)] == '.')
            {
                mapState.X = newX;
                mapState.Y = newY;
                mapState.Direction = newDir;
            }
        }

        private int CubeNum(int x, int y, int faceSize)
            => 3 * (y / faceSize) + (x / faceSize);

        private MapState Parse()
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
                        map[(x, y)] = curr;
                        if (startX < 0 && map[(x, y)] == '.')
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

            return new MapState(startX, startY, map, path);
        }
    }
}