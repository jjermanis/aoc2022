using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AoC2022
{
    public class Day15 : DayBase, IDay
    {
        // TODO This solves 15-2, but does not work for the sample problem.
        // There are "obvious" ways to fix... but adds complexity needed for only the sample?
        // Probably can fix by checking if any possible points is inside of a sensor diamond.

        private struct Sensor
        {
            public readonly int ID;
            public readonly int X;
            public readonly int Y;
            public readonly int ClosestBeaconX; 
            public readonly int ClosestBeaconY;
            public readonly int Distance;

            public Sensor(
                int id,
                int x, int y,
                int closestBeaconX, int closestBeaconY)
            {
                ID = id;
                X = x;
                Y = y;
                ClosestBeaconX = closestBeaconX;
                ClosestBeaconY = closestBeaconY;

                Distance = Math.Abs(closestBeaconX - x) +
                    Math.Abs(closestBeaconY - Y);
            }

            public IEnumerable<Line> GetLines()
            {
                yield return new Line(ID, X - Distance, Y, X, Y - Distance);
                yield return new Line(ID, X - Distance, Y, X, Y + Distance);
                yield return new Line(ID, X, Y - Distance, X + Distance, Y);
                yield return new Line(ID, X, Y + Distance, X + Distance, Y);
            }
        }

        private struct Line
        {
            public readonly int SensorID;
            public readonly int X1;
            public readonly int Y1;
            public readonly int X2;
            public readonly int Y2;
            private readonly int Slope;

            public Line(int sensorID, int x1, int y1, int x2, int y2)
            {
                if (x1 >= x2)
                    throw new Exception("Point needs to be on the left");
                SensorID = sensorID;
                X1 = x1;
                Y1 = y1;
                X2 = x2;
                Y2 = y2;

                Slope = Y2 > Y1 ? 1 : -1;
            }

            public (int? x, int? y) Intersect(Line other)
            {
                (int?, int?) noResult = (null, null);

                if (this.SensorID == other.SensorID)
                    return noResult;
                if (this.Slope == other.Slope)
                    return noResult;

                int thisYint = this.X1 - (this.Slope * this.Y1);
                int otherYint = other.X1 - (other.Slope * other.Y1);
                if ((thisYint + otherYint) % 2 != 0)
                    return noResult;

                int xIntersect = (thisYint + otherYint) / 2;
                if (xIntersect >= this.X1 && xIntersect <= this.X2 &&
                    xIntersect >= other.X1 && xIntersect <= other.X2)
                {
                    var result = (xIntersect, Y1 + ((xIntersect - X1) * Slope));
                    return result;
                }
                return noResult;
            }
            public override string ToString()
                => $"({X1},{Y1})=>({X2},{Y2}), slope={Slope}";
        }

        private readonly IEnumerable<string> _lines;

        public Day15(string filename)
            => _lines = TextFileLines(filename);

        public Day15() : this("Day15.txt")
        {
        }

        public void Do()
        {
            Console.WriteLine($"{nameof(NoBeaconCount)}: {NoBeaconCountRow2M()}");
            Console.WriteLine($"{nameof(DistressTuningFrequency)}: {DistressTuningFrequency4M()}");
        }

        public int NoBeaconCountRow2M()
            => NoBeaconCount(2000000);

        public int NoBeaconCount(int rowNum)
        {
            var sensors = Parse();

            int minX = int.MaxValue;
            int maxX = int.MinValue;
            foreach (var sensor in sensors)
            {
                minX = Math.Min(minX, sensor.X-sensor.Distance);
                maxX = Math.Max(maxX, sensor.X+sensor.Distance);
            }
            var specificRow = new int[maxX - minX];
            foreach (var sensor in sensors)
            {
                int vDist = Math.Abs(sensor.Y - rowNum);
                if (vDist < sensor.Distance)
                {
                    var radius = sensor.Distance - vDist;
                    for (var x = sensor.X - radius; x <= sensor.X + radius; x++)
                        specificRow[x-minX] = 1;
                }
            }
            foreach (var sensor in sensors)
            {
                if (sensor.ClosestBeaconY == rowNum)
                    specificRow[sensor.ClosestBeaconX-minX] = 2;
            }
            var result = 0;
            foreach (var v in specificRow)
                if (v == 1)
                    result++;
            return result;
        }

        public long DistressTuningFrequency4M()
            => DistressTuningFrequency(4000000);

        public long DistressTuningFrequency(int limit)
        {
            var sensors = Parse();

            // Create a list of the lines of the boundaries for each sensor
            var lines = new List<Line>();
            foreach (var sensor in sensors)
                foreach (var line in sensor.GetLines())
                    lines.Add(line);

            // Create a map of the intersections between lines from different sensors
            var intersects = new HashSet<(int? x, int? y)>();
            for (int i=0; i<lines.Count; i++)
                for (int j=i+1; j<lines.Count; j++)
                {
                    var inter = lines[i].Intersect(lines[j]);
                    if (inter.x != null &&
                        inter.x >= 0 && inter.x <= limit &&
                        inter.y >= 0 && inter.y <= limit)
                        intersects.Add(inter);
                }
            
            // Look for spots that are surrounded by intersections on each side
            foreach((var x, var y) in intersects)
            {
                if (intersects.Contains((x - 1, y + 1)) &&
                    intersects.Contains((x + 1, y + 1)) &&
                    intersects.Contains((x, y + 2)))
                {
                    long result = 4000000 * (long)x + (long)y + 1;
                    return result;
                }
            }
            throw new Exception("No answer found");
        }

        private IList<Sensor> Parse()
        {
            var result = new List<Sensor>();
            var ID = 0;
            foreach (var line in _lines)
            {
                var m = Regex.Match(line, @"Sensor at x=(-?\d*), y=(-?\d*): closest beacon is at x=(-?\d*), y=(-?\d*)");
                var sensorX = int.Parse(m.Groups[1].Value);
                var sensorY = int.Parse(m.Groups[2].Value);
                var beaconX = int.Parse(m.Groups[3].Value);
                var beaconY = int.Parse(m.Groups[4].Value);

                result.Add(new Sensor(ID, sensorX, sensorY, beaconX, beaconY));
                ID++;
            }
            return result;
        }
    }
}