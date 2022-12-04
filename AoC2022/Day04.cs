using System;
using System.Collections.Generic;

namespace AoC2022
{
    public class Day04 : DayBase, IDay
    {
        private struct Assignment
        {
            public readonly int Left;
            public readonly int Right;

            public Assignment(string text)
            {
                var values = text.Split('-');
                Left = int.Parse(values[0]);
                Right = int.Parse(values[1]);
            }
        }

        private readonly IEnumerable<string> _lines;

        public Day04(string filename)
            => _lines = TextFileLines(filename);

        public Day04() : this("Day04.txt")
        {
        }

        public void Do()
        {
            Console.WriteLine($"{nameof(FullyContainedCount)}: {FullyContainedCount()}");
            Console.WriteLine($"{nameof(OverlapCount)}: {OverlapCount()}");
        }

        public int FullyContainedCount()
        {
            var result = 0;
            foreach(var line in _lines)
            {
                (var left, var right) = ParseAssignments(line);
                if (IsFullyContained(left, right))
                    result++;
            }
            return result;
        }

        public int OverlapCount()
        {
            var result = 0;
            foreach (var line in _lines)
            {
                (var left, var right) = ParseAssignments(line);
                if (IsPairOverlap(left, right))
                    result++;
            }
            return result;
        }
        private (Assignment, Assignment) ParseAssignments(string text)
        {
            var splitTexts = text.Split(',');
            var a0 = new Assignment(splitTexts[0]);
            var a1 = new Assignment(splitTexts[1]);

            // In the tuple, return the assignment that starts from the left first.
            // If they both start from the same spot on the left, return the bigger one first.
            if (a0.Left < a1.Left)
                return (a0, a1);
            if (a1.Left < a0.Left)
                return (a1, a0);
            if (a0.Right > a1.Right)
                return (a0, a1);
            return (a1, a0);
        }
        private bool IsFullyContained(Assignment leftAssignent, Assignment rightAssignment)
            => leftAssignent.Left <= rightAssignment.Left 
            && leftAssignent.Right >= rightAssignment.Right;

        private bool IsPairOverlap(Assignment leftAssignent, Assignment rightAssignment)
            => leftAssignent.Right >= rightAssignment.Left;
    }
}