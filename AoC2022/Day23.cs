using System;
using System.Collections.Generic;

namespace AoC2022
{
    public class Day23 : DayBase, IDay
    {
        // TODO this runs in about 2 seconds. Are their efficiencies?
        // HasNoNeighbors() has some redunancies. That's one case.

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

        private HashSet<(int x, int y)> ParseMap()
        {
            var result = new HashSet<(int x, int y)>();
            var y = 0;
            foreach (var line in _lines)
            {
                for (var x = 0; x < line.Length; x++)
                {
                    if (line[x] == '#')
                        result.Add((x, y));
                }
                y++;
            }
            return result;
        }

        private List<(int x, int y)> ElvesToMove(HashSet<(int x, int y)> elves)
        {
            var result = new List<(int x, int y)>();
            foreach (var (currX, currY) in elves)
                if (!HasNoNeighbors(elves, currX, currY))
                    result.Add((currX, currY));
            return result;
        }

        private bool HasNoNeighbors(HashSet<(int x, int y)> elves, int x, int y)
        {
            foreach (var move in MOVE_ORDER)
                foreach (var vector in move)
                    if (elves.Contains((x + vector.x, y + vector.y)))
                        return false;
            return true;
        }

        private void MoveElves(
            List<(int x, int y)> elvesToMove,
            HashSet<(int x, int y)> elves,
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
                        if (elves.Contains((currX + dX, currY + dY)))
                            collisionFound = true;
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
                    elves.Remove((currX, currY));
                    elves.Add((newX, newY));
                }
            }
        }

        private int FreeSpacesInMap(HashSet<(int x, int y)> elves)
        {
            int minX = int.MaxValue;
            int maxX = int.MinValue;
            int minY = int.MaxValue;
            int maxY = int.MinValue;

            foreach (var (x, y) in elves)
            {
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