using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC2022
{
    public class Day17 : DayBase, IDay
    {
        private readonly IEnumerable<string> _lines;

        public Day17(string filename)
            => _lines = TextFileLines(filename);

        public Day17() : this("Day17.txt")
        {
        }

        public void Do()
        {
            Console.WriteLine($"{nameof(Part1)}: {Part1()}");
            Console.WriteLine($"{nameof(Part2)}: {Part2()}");
        }

        public int Part1()
        {
            return 0;
        }

        public int Part2()
        {
            return 0;
        }
    }
}