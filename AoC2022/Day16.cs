using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC2022
{
    public class Day16 : DayBase, IDay
    {

        // TODO: 16-1 runs in about 6 seconds. Are there faster approaches?

        private struct Room
        {
            public readonly string Name;
            public readonly int ValveRate;
            public readonly List<string> Neighbors;

            public Room(string name, int rate, string neighbors)
            {
                Name = name;
                ValveRate = rate;
                Neighbors = neighbors.Split(", ").ToList();
            }
            public override string ToString()
                => Name;
        }

        private readonly IEnumerable<string> _lines;

        public Day16(string filename)
            => _lines = TextFileLines(filename);

        public Day16() : this("Day16.txt")
        {
        }

        public void Do()
        {
            Console.WriteLine($"{nameof(MaxPressureSolo)}: {MaxPressureSolo()}");
            Console.WriteLine($"{nameof(Part2)}: {Part2()}");
        }

        public int MaxPressureSolo() 
            => MaxPressureReleasedSolo(Parse(), "AA", 30);

        private int MaxPressureReleasedSolo(
            IDictionary<string, Room> rooms,
            string currRoomName,
            int minutesRemaining)
        {
            var cache = new Dictionary<(string, string, int), (int, int)>();
            var result = MaxPressureReleased(
                rooms, currRoomName, "", 0, minutesRemaining,
                cache);
            return result;
        }

        private int MaxPressureReleased(
            in IDictionary<string, Room> rooms,
            in string currRoomName,
            in string valvesOpen,
            in int totalPressureReleased,
            in int minutesRemaining,
            IDictionary<(string, string, int), (int curr, int max)> cache)
        {
            if (cache.ContainsKey((currRoomName, valvesOpen, minutesRemaining)))
            {
                var cacheVal = cache[(currRoomName, valvesOpen, minutesRemaining)];
                if (cacheVal.curr >= totalPressureReleased)
                    return cacheVal.max;
            }

            var delta = 0;
            var openValveList = new List<string>();
            if (valvesOpen.Length > 0)
            {
                openValveList = valvesOpen.Split(',').ToList();
                foreach (var valve in openValveList)
                    delta += rooms[valve].ValveRate;
            }
            var updatedPressureReleased = totalPressureReleased + delta;

            if (minutesRemaining == 1)
            {
                cache[(currRoomName, valvesOpen, minutesRemaining)] = (totalPressureReleased, updatedPressureReleased);
                return updatedPressureReleased;
            }

            var max = updatedPressureReleased;

            if (!valvesOpen.Contains(currRoomName) && rooms[currRoomName].ValveRate > 0)
            {
                var temp = new List<string>(openValveList);
                temp.Add(currRoomName);
                temp.Sort();
                var newValvesOpen = temp[0];
                for (int i = 1; i < temp.Count; i++)
                {
                    newValvesOpen += $",{temp[i]}";
                }
                var curr = MaxPressureReleased(rooms, currRoomName, newValvesOpen, updatedPressureReleased, minutesRemaining - 1, cache);
                if (curr > max)
                    max = curr;
            }
            foreach (var neighbor in rooms[currRoomName].Neighbors)
            {
                var curr = MaxPressureReleased(rooms, neighbor, valvesOpen, updatedPressureReleased, minutesRemaining-1, cache);
                if (curr > max)
                    max = curr;
            }

            cache[(currRoomName, valvesOpen, minutesRemaining)] = (totalPressureReleased, max);
            return max;
        } 

        public int Part2()
        {
            return 0;
        }

        private IDictionary<string, Room> Parse()
        {
            var result = new Dictionary<string, Room>();
            foreach (var line in _lines)
            {
                var m = Regex.Match(line, @"Valve ([A-Z][A-Z]) has flow rate=(\d*); tunnels? leads? to valves? (.*)$");
                var name = m.Groups[1].Value;
                var rate = int.Parse(m.Groups[2].Value);
                var rawAdjacentRooms = m.Groups[3].Value;

                result[name] = new Room(name, rate, rawAdjacentRooms);
            }
            return result;
        }
    }
}