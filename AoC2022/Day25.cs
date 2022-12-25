using System;
using System.Collections.Generic;
using System.Text;

namespace AoC2022
{
    public class Day25 : DayBase, IDay
    {
        private readonly IEnumerable<string> _lines;

        public Day25(string filename)
            => _lines = TextFileLines(filename);

        public Day25() : this("Day25.txt")
        {
        }

        public void Do()
        {
            Console.WriteLine($"{nameof(Part1)}: {Part1()}");
            Console.WriteLine($"{nameof(Part2)}: {Part2()}");
        }

        public string Part1()
        {
            var decimalSum = DecimalSum();
            return SnafuValue(decimalSum);
        }

        public int Part2()
        {
            // As usual, there is not a Part2 for Day 25 (and Day 25 only)
            return 0;
        }

        private long DecimalSum()
        {
            long result = 0;
            foreach (var line in _lines)
            {
                long value = 0;
                for (int i = 0; i < line.Length; i++)
                {
                    var curr = line[i] switch
                    {
                        '2' => 2,
                        '1' => 1,
                        '0' => 0,
                        '-' => -1,
                        '=' => -2,
                        _ => throw new Exception()
                    };
                    value *= 5;
                    value += curr;
                }
                result += value;
            }
            return result;
        }

        private string SnafuValue(long decimalVal)
        {
            var base5 = new List<int>();
            while (decimalVal > 0)
            {
                var digit = (int)(decimalVal % 5);
                base5.Add(digit);
                decimalVal /= 5;
            }
            for (int i = 0; i < base5.Count; i++)
            {
                if (base5[i] > 2)
                {
                    base5[i] -= 5;
                    if (i + 1 < base5.Count)
                        base5[i+1]++;
                    else
                        base5.Add(1);
                }
            }
            base5.Reverse();

            var sb = new StringBuilder();
            foreach(var digit in base5)
            {
                char curr = digit switch
                {
                    2 => '2',
                    1 => '1',
                    0 => '0',
                    -1 => '-',
                    -2 => '=',
                    _ => throw new Exception()
                };
                sb.Append(curr);
            }
            return sb.ToString();
        }
    }
}