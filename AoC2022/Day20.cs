using System;
using System.Collections.Generic;

namespace AoC2022
{
    public class Day20 : DayBase, IDay
    {
        // TODO - takes over a second, which is... OK
        // String conversion is used to track order - try using a pure long? Or a Tuple?

        private readonly IEnumerable<string> _lines;

        public Day20(string filename)
            => _lines = TextFileLines(filename);

        public Day20() : this("Day20.txt")
        {
        }

        public void Do()
        {
            Console.WriteLine($"{nameof(GroveSum)}: {GroveSum()}");
            Console.WriteLine($"{nameof(DecryptedGroveSum)}: {DecryptedGroveSum()}");
        }

        public long GroveSum()
        {
            var list = ParseList();
            var moved = MoveNumbers(list, 1);
            return SumOfCoords(moved);
        }

        public long DecryptedGroveSum()
        {
            var list = ParseList(811589153);
            var moved = MoveNumbers(list, 10);
            return SumOfCoords(moved);
        }

        private List<long> ParseList()
            => ParseList(1);

        private List<long> ParseList(long key)
        {
            var result = new List<long>();
            foreach (var line in _lines)
                result.Add(key * long.Parse(line));
            return result;
        }

        private List<long> MoveNumbers(
            IList<long> numberOrder,
            int repetitions)
        {
            var detailList = new List<string>();
            for (int i = 0; i < numberOrder.Count; i++)
                detailList.Add($"{numberOrder[i]}_{i}");
            var listLen = numberOrder.Count;

            for (int c = 0; c < repetitions; c++)
            {
                for (int i = 0; i < listLen; i++)
                {
                    var detailName = $"{numberOrder[i]}_{i}";
                    var index = detailList.IndexOf(detailName);
                    detailList.RemoveAt(index);
                    var delta = numberOrder[i];
                    var newIndex = index + delta;

                    if (newIndex < 0)
                    {
                        // In C#, mod operator on a negative returns a negative.
                        // These steps take that mod, and turn it to the postive equivalent.
                        var temp = newIndex % (listLen - 1);
                        newIndex = temp + listLen - 1;
                    }    
                    if (newIndex >= listLen)
                        newIndex = newIndex % (listLen - 1);
                    detailList.Insert((int)newIndex, detailName);
                }
            }

            // Strip away the detailed name, and get back to the original numbers
            var result = new List<long>();
            foreach (var val in detailList)
                result.Add(long.Parse(val.Split('_')[0]));
            return result;
        }

        private long SumOfCoords(IList<long> numbers)
        {
            var zIndex = numbers.IndexOf(0);
            long result = 0L;
            for (int i = 1; i <= 3; i++)
                result += numbers[(1000 * i + zIndex) % numbers.Count];
            return result;
        }
    }
}