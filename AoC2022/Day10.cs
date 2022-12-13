using System;
using System.Collections.Generic;

namespace AoC2022
{
    public class Day10 : DayBase, IDay
    {
        private readonly IEnumerable<string> _lines;

        public Day10(string filename)
            => _lines = TextFileLines(filename);

        public Day10() : this("Day10.txt")
        {
        }

        public void Do()
        {
            Console.WriteLine($"{nameof(InterestingSignalSum)}: {InterestingSignalSum()}");
            Console.WriteLine($"{nameof(ImageRender)}: {ImageRender()}");
        }

        public int InterestingSignalSum()
        {
            var duringValues = ValuesOverTime();

            var result = 0;
            for (int i = 20; i <= 220; i+= 40)
            {
                var curr = i * duringValues[i];
                result += curr;
            }
            return result;
        }

        public int ImageRender()
        {
            var duringValues = ValuesOverTime();
            for (int i = 0; i < duringValues.Count - 1; i++)
            {
                var currX = duringValues[i + 1];
                var currPixel = i % 40;
                if (currPixel == 0)
                    Console.WriteLine();
                if (Math.Abs(currX - currPixel) <= 1)
                    Console.Write('W');
                else
                    Console.Write(' ');
            }
            Console.WriteLine();

            // This problem is not suited to the normal pattern
            return -1;
        }

        private List<int> ValuesOverTime()
        {
            // Put an unused value at index 0 - the problem states things as starting at
            // index 1, making this code easier to read.
            var result = new List<int>() { int.MinValue };
            var x = 1;
            var cycleNum = 1;

            foreach (var instruction in _lines)
            {
                var op = instruction[0..4];
                switch (op)
                {
                    case "noop":
                        result.Add(x);
                        cycleNum++;
                        break;
                    case "addx":
                        result.Add(x);
                        result.Add(x);
                        var operand = int.Parse(instruction[4..]);
                        x += operand;
                        cycleNum += 2;
                        break;
                    default:
                        throw new Exception($"Undefined op: {op}");
                }
            }
            return result;
        }
    }
}