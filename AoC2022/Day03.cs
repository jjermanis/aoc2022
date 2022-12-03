using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC2022
{
    public class Day03 : DayBase, IDay
    {
        private readonly IList<string> _lines;

        public Day03(string filename)
            => _lines = TextFileStringList(filename);

        public Day03() : this("Day03.txt")
        {
        }

        public void Do()
        {
            Console.WriteLine($"{nameof(SumMisplacedItems)}: {SumMisplacedItems()}");
            Console.WriteLine($"{nameof(SumBadges)}: {SumBadges()}");
        }

        public int SumMisplacedItems()
        {
            var result = 0;
            foreach(var rucksack in _lines)
            {
                var duplicate = FindMisplacedItem(rucksack);
                result += ItemScore(duplicate);
            }
            return result;
        }

        public int SumBadges()
        {
            var result = 0;
            for (var x = 0; x < _lines.Count; x+=3)
            {
                var s1 = GenerateRucksackHashSet(_lines[x]);
                var s2 = GenerateRucksackHashSet(_lines[x+1]);
                var s3 = GenerateRucksackHashSet(_lines[x+2]);
                s1.IntersectWith(s2);
                s1.IntersectWith(s3);
                result += ItemScore(s1.ElementAt(0));
            }
            return result;
        }

        private char FindMisplacedItem(string rucksack)
        {
            var mid = rucksack.Length / 2;
            var left = GenerateRucksackHashSet(rucksack[0..mid]);
            for (var i = mid; i < rucksack.Length; i++)
            {
                if (left.Contains(rucksack[i]))
                    return rucksack[i];
            }
            throw new Exception("Not found");
        }

        private int ItemScore(char c)
            => char.IsLower(c) ? (c - 'a' + 1) : (c - 'A' + 27);

        private HashSet<char> GenerateRucksackHashSet(string x)
        {
            var result = new HashSet<char>();
            foreach (char c in x)
            {
                if (!result.Contains(c))
                    result.Add(c);
            }
            return result;
        }
    }
}