using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC2022
{
    public class Day16 : DayBase, IDay
    {
        // TODO yet another case that works for real, and not the sample. Approach for 16-2 takes advantage
        // of something that does not hold for the sample (because it is small). 
        // Current approach for 16-2: do all 26 moves with one person, then 26 with another.
        // To work on sample: attempt to approach both people at same time.

        private readonly Dictionary<string, Dictionary<string, int>> _map;
        private readonly Dictionary<string, int> _valveRate;

        public Day16(string filename)
        {
            var lines = TextFileLines(filename);
            _map = new Dictionary<string, Dictionary<string, int>>();
            _valveRate = new Dictionary<string, int>();

            var internalMap = new Dictionary<string, List<string>>();
            foreach (var line in lines)
            {
                var m = Regex.Match(line, @"Valve ([A-Z][A-Z]) has flow rate=(\d*); tunnels? leads? to valves? (.*)$");
                var name = m.Groups[1].Value;
                var rate = int.Parse(m.Groups[2].Value);
                var rawAdjacentRooms = m.Groups[3].Value.Split(", ").ToList();

                _valveRate[name] = rate;
                internalMap[name] = rawAdjacentRooms;
            }

            foreach (var roomName in internalMap.Keys)
            {
                if (roomName != "AA" && _valveRate[roomName] == 0)
                    continue;

                var currDistance = new Dictionary<string, int>();
                var todo = new Queue<(string name, int time)>();
                todo.Enqueue((roomName, 0));
                while (todo.Count > 0)
                {
                    var curr = todo.Dequeue();
                    foreach (var neighbor in internalMap[curr.name])
                    {
                        if (!currDistance.ContainsKey(neighbor))
                        {
                            currDistance[neighbor] = curr.time + 1;
                            todo.Enqueue((neighbor, curr.time + 1));
                        }
                    }
                }
                _map[roomName] = currDistance;
            }
        }

        public Day16() : this("Day16.txt")
        {
        }

        public void Do()
        {
            Console.WriteLine($"{nameof(MaxPressureSolo)}: {MaxPressureSolo()}");
            Console.WriteLine($"{nameof(MaxpressureDuo)}: {MaxpressureDuo()}");
        }

        public int MaxPressureSolo()
        {
            (var max, _) = MaxPressureReleased(AllUsefulValves(), 30);
            return max;
        }

        public int MaxpressureDuo()
        {
            var (max1, remainingValves) = MaxPressureReleased(AllUsefulValves(), 26);
            var (max2, _) = MaxPressureReleased(remainingValves, 26);
            return max1+max2;
        }

        private List<string> AllUsefulValves()
        {
            var result = new List<string>();
            foreach (var valve in _valveRate)
                if (valve.Value > 0)
                    result.Add(valve.Key);
            return result;
        }

        private (int, List<string>) MaxPressureReleased(List<string> valvesAvailable, int totalTurns)
            => MaxPressureReleased(valvesAvailable, "AA", totalTurns, 0, 0);

        private (int, List<string>) MaxPressureReleased(
            List<string> valvesAvailable,
            string currLocation,
            int timeRemaining,
            int currRate,
            int pressureReleased)
        {
            var max = pressureReleased + (timeRemaining * currRate);
            var bestValves = valvesAvailable;
            foreach (var valve in valvesAvailable)
            {
                var valveTime = _map[currLocation][valve] + 1;
                var newTime = timeRemaining - valveTime;
                if (newTime > 0)
                {
                    var vaCopy = new List<string>(valvesAvailable);
                    vaCopy.Remove(valve);
                    var nextValveRate = _valveRate[valve];
                    var (currValue, openValves) = MaxPressureReleased(
                        vaCopy, valve, newTime, currRate + nextValveRate, pressureReleased + (valveTime * currRate));
                    if (currValue > max)
                    {
                        max = currValue;
                        bestValves = openValves;
                    }
                }
            }
            return (max, bestValves);
        }

    }
}