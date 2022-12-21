using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC2022
{
    public class Day20 : DayBase, IDay
    {
        private readonly IEnumerable<string> _lines;

        public Day20(string filename)
            => _lines = TextFileLines(filename);

        public Day20() : this("Day20.txt")
        {
        }

        public void Do()
        {
            Console.WriteLine($"{nameof(Part1)}: {Part1()}");
            Console.WriteLine($"{nameof(Part2)}: {Part2()}");
        }

        public int Part1()
        {
            var list = ParseList();
            var moved = MoveNumbers(list);
            var zIndex = moved.IndexOf(0);
            var v1 = moved[(zIndex + 1000) % list.Count];
            var v2 = moved[(zIndex + 2000) % list.Count];
            var v3 = moved[(zIndex + 3000) % list.Count];
            return v1 + v2 + v3;
        }

        public int Part2()
        {
            
            return 0;
        }

        private List<int> ParseList()
        {
            var result = new List<int>();
            foreach (var line in _lines)
                result.Add(int.Parse(line));
            return result;
        }

        private List<int> MoveNumbers(IList<int> numbers)
        {
            var order = new List<int>(numbers);
            var formalList = new List<string>();
            for (int i = 0; i < order.Count; i++)
                formalList.Add($"{order[i]}_{i}");
            var listLen = order.Count;
            for (int i = 0; i < order.Count; i++)
            {
                var name = $"{order[i]}_{i}";
                var index = formalList.IndexOf(name);
                formalList.RemoveAt(index);
                var delta = order[i];
                var newIndex = index + delta;
                while (newIndex < 0)
                    newIndex = newIndex + (listLen - 1);
                if (newIndex >= listLen)
                    newIndex = newIndex % (listLen - 1);
                formalList.Insert(newIndex, name);
            }

            var result = new List<int>();
            foreach (var val in formalList)
                result.Add(int.Parse(val.Split('_')[0]));
            return result;
        }
    }
}