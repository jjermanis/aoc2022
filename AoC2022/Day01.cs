using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC2022
{
    public class Day01 : DayBase, IDay
    {
        private readonly IList<string> _lines;

        public Day01(string filename)
            => _lines = TextFileStringList(filename);

        public Day01() : this("Day01.txt")
        {
        }

        public void Do()
        {
            Console.WriteLine($"{nameof(MaxElfCalories)}: {MaxElfCalories()}");
            Console.WriteLine($"{nameof(TopThreeElfCalories)}: {TopThreeElfCalories()}");
        }

        public int MaxElfCalories()
            => GetElfCalories().Max();

        public int TopThreeElfCalories()
        {
            var list = new List<int>(GetElfCalories());
            var top = list.OrderByDescending(x => x).ToList();
            return top[0] + top[1] + top[2];
        }

        private IEnumerable<int> GetElfCalories()
        {
            int subtotal = 0;
            foreach (var entry in _lines)
            {
                if (entry.Length == 0)
                {
                    yield return subtotal;
                    subtotal = 0;
                }
                else
                    subtotal += int.Parse(entry);
            }
            yield return subtotal;
        }
    }
}