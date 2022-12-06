using System;
using System.Collections.Generic;

namespace AoC2022
{
    public class Day06 : DayBase, IDay
    {
        private readonly string _data;

        public Day06(string filename)
            => _data = TextFile(filename);

        public Day06() : this("Day06.txt")
        {
        }

        public void Do()
        {
            Console.WriteLine($"{nameof(StartOfPacket)}: {StartOfPacket()}");
            Console.WriteLine($"{nameof(StartOfMessage)}: {StartOfMessage()}");
        }

        public int StartOfPacket()
            => FirstIndexWithoutRepeats(4);

        public int StartOfMessage()
            => FirstIndexWithoutRepeats(14);

        private int FirstIndexWithoutRepeats(int length)
        {
            for (var i = length; i <= _data.Length; i++)
            {
                var markers = new HashSet<char>();
                var match = false;
                for (var j = 1; j <= length; j++)
                {
                    var curr = _data[i - j];
                    if (markers.Contains(curr))
                    {
                        match = true;
                        break;
                    }
                    markers.Add(curr);
                }
                if (!match)
                    return i;
            }
            throw new Exception("not found");
        }
    }
}