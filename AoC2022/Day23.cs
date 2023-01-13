using System;
using System.Collections.Generic;

namespace AoC2022
{
    public class Day23 : DayBase, IDay
    {
        // TODO. THis is decently fast. Might this be even faster using an array instead of the HashSet?

        private static readonly IList<(int x, int y)> NORTH 
            = new List<(int x, int y)>(){ (0, -1), (1, -1), (-1, -1) };
        private static readonly IList<(int x, int y)> SOUTH
            = new List<(int x, int y)>() { (0, 1), (1, 1), (-1, 1) };
        private static readonly IList<(int x, int y)> WEST
            = new List<(int x, int y)>() { (-1, 0), (-1, -1), (-1, 1) };
        private static readonly IList<(int x, int y)> EAST
            = new List<(int x, int y)>() { (1, 0), (1, -1), (1, 1) };
        private static readonly IList<IList<(int x, int y)>> MOVE_ORDER
            = new List<IList<(int x, int y)>>() { NORTH, SOUTH, WEST, EAST };

        private readonly IEnumerable<string> _lines;

        public Day23(string filename)
            => _lines = TextFileLines(filename);

        public Day23() : this("Day23.txt")
        {
        }

        public void Do()
        {
            Console.WriteLine($"{nameof(SpaceAfter10Moves)}: {SpaceAfter10Moves()}");
            Console.WriteLine($"{nameof(FirstStationaryRound)}: {FirstStationaryRound()}");
        }

        public int SpaceAfter10Moves()
        {
            var elves = ParseMap();
            for (int round=0; round < 10; round++)
            {
                var elvesToMove = ElvesToMove(elves);
                MoveElves(elvesToMove, elves, round);
            }
            return FreeSpacesInMap(elves);
        }

        public int FirstStationaryRound()
        {
            var elves = ParseMap();
            var round = 0;
            while (true)
            {
                var elvesToMove = ElvesToMove(elves);
                if (elvesToMove.Count == 0)
                    return round + 1;

                MoveElves(elvesToMove, elves, round);
                round++;
            }
        }

        private int CoordToHashKey(int x, int y)
            => 1024 * (x + 512) + (y + 512);
        private (int x, int y) CoordFromHashKey(int key)
            => (key / 1024 - 512, key % 1024 - 512);

        private HashSet<int> ParseMap()
        {
            var result = new HashSet<int>();
            var y = 0;
            foreach (var line in _lines)
            {
                for (var x = 0; x < line.Length; x++)
                {
                    if (line[x] == '#')
                        result.Add(CoordToHashKey(x, y));
                }
                y++;
            }
            return result;
        }

        private List<(int x, int y)> ElvesToMove(HashSet<int> elves)
        {
            var result = new List<(int x, int y)>();
            foreach (var hashKey in elves)
            {
                var (currX, currY) = CoordFromHashKey(hashKey);
                if (!HasNoNeighbors(elves, currX, currY))
                    result.Add((currX, currY));
            }
            return result;
        }

        private bool HasNoNeighbors(HashSet<int> elves, int x, int y)
        {
            for (var dx = -1; dx <= 1; dx++)
                for (var dy = -1; dy <= 1; dy++)
                    if (dx != 0 || dy != 0)
                    {
                        var currKey = CoordToHashKey(x + dx, y + dy);
                        if (elves.Contains(currKey))
                            return false;
                    }
            return true;
        }

        private void MoveElves(
            List<(int x, int y)> elvesToMove,
            HashSet<int> elves,
            int roundNum)
        {
            var proposedMoves = new Dictionary<(int x, int y), (int x, int y)>();
            foreach (var (currX, currY) in elvesToMove)
            {
                int freeDirection = -1;
                for (int i = roundNum; i < roundNum + 4; i++)
                {
                    var curMove = MOVE_ORDER[i % 4];
                    var collisionFound = false;
                    foreach (var (dX, dY) in curMove)
                    {
                        var currKey = CoordToHashKey(currX + dX, currY + dY);
                        if (elves.Contains(currKey))
                            collisionFound = true;
                    }
                    if (!collisionFound)
                    {
                        freeDirection = i % 4;
                        break;
                    }
                }
                if (freeDirection >= 0)
                {
                    var (dX, dY) = MOVE_ORDER[freeDirection][0];
                    if (!proposedMoves.ContainsKey((currX + dX, currY + dY)))
                        proposedMoves[(currX + dX, currY + dY)] = (currX, currY);
                    else
                        proposedMoves[(currX + dX, currY + dY)] = (int.MinValue, int.MinValue);
                }
            }
            foreach (var (newX, newY) in proposedMoves.Keys)
            {
                var (currX, currY) = proposedMoves[(newX, newY)];
                if (currX != int.MinValue)
                {
                    elves.Remove(CoordToHashKey(currX, currY));
                    elves.Add(CoordToHashKey(newX, newY));
                }
            }
        }

        private int FreeSpacesInMap(HashSet<int> elves)
        {
            int minX = int.MaxValue;
            int maxX = int.MinValue;
            int minY = int.MaxValue;
            int maxY = int.MinValue;

            foreach (var key in elves)
            {
                var (x, y) = CoordFromHashKey(key);
                minX = Math.Min(minX, x);
                maxX = Math.Max(maxX, x);
                minY = Math.Min(minY, y);
                maxY = Math.Max(maxY, y);
            }
            var area = (maxX-minX+1)*(maxY - minY + 1);
            return area - elves.Count;
        }
    }
}