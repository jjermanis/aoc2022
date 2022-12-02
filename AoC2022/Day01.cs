using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC2022
{
    public class Day01 : DayBase, IDay
    {
        private readonly IList<string> _calorieEntries;

        public Day01(string filename)
            => _calorieEntries = TextFileStringList(filename);

        public Day01() : this("Day01.txt")
        {
        }

        public void Do()
        {
            Console.WriteLine($"Part1: {MaxElfCalories()}");
            Console.WriteLine($"Part2: {TopThreeElfCalories()}");
        }

        public int MaxElfCalories()
            => GetElfCalories().Max();


        public int TopThreeElfCalories()
        {
            var list = new List<int>(GetElfCalories());
            list.Sort();
            list.Reverse();
            return list[0] + list[1] + list[2];
        }

        private IEnumerable<int> GetElfCalories()
        {
            var subtotal = 0;
            foreach (var entry in _calorieEntries)
            {
                if (entry.Length == 0)
                {
                    yield return subtotal;
                    subtotal = 0;
                }
                else
                {
                    subtotal += int.Parse(entry);
                }    
            }
            yield return subtotal;
        }
    }
}