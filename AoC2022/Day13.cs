using System;
using System.Collections.Generic;

namespace AoC2022
{
    public class Day13 : DayBase, IDay
    {
        private enum PacketOrder
        {
            Right = -1,
            Wrong = 1,
            Equal = 0
        };

        private abstract class PacketData { }

        private class PacketInteger : PacketData
        {
            public readonly int Value;

            public PacketInteger(int value)
                => Value = value;
            public override string ToString()
                => Value.ToString();
        }
        private class PacketList : PacketData
        {
            public List<PacketData> Items = new List<PacketData>();
            public string Name;

            public PacketList()
            {
            }

            public PacketList(PacketInteger value)
            {
                Items.Add(new PacketInteger(value.Value));
            }
        }

        private readonly IList<string> _lines;

        public Day13(string filename)
            => _lines = TextFileStringList(filename);

        public Day13() : this("Day13.txt")
        {
        }

        public void Do()
        {
            Console.WriteLine($"{nameof(Part1)}: {Part1()}");
            Console.WriteLine($"{nameof(Part2)}: {Part2()}");
        }

        public int Part1()
        {
            var result = 0;
            var pairId = 0;
            for (var i = 0; i < _lines.Count; i += 3)
            {
                pairId++;
                var left = ParsePakcetList(i);
                var right = ParsePakcetList(i+1);

                if (IsRightOrder(left, right))
                    result += pairId;
            }
            return result;
        }

        public int Part2()
        {
            const string MARKER2 = "[[2]]";
            const string MARKER6 = "[[6]]";

            var packets = new List<PacketList>();
            for (var i = 0; i < _lines.Count; i += 3)
            {
                packets.Add(ParsePakcetList(i));
                packets.Add(ParsePakcetList(i+1));
            }
            packets.Add(ParsePakcetList(MARKER2));
            packets.Add(ParsePakcetList(MARKER6));
            packets.Sort(ComparePacketLists);

            int loc2 = -1;
            int loc6 = -1;
            for (var x = 0; x < packets.Count; x++)
            {
                var p = packets[x];
                if (p.Name == MARKER2)
                    loc2 = x + 1;
                if (p.Name == MARKER6)
                    loc6 = x + 1;
            }
            return loc2 * loc6;
        }

        private bool IsRightOrder(PacketList left, PacketList right)
            => GetPacketOrder(left, right) == PacketOrder.Right;

        private int ComparePacketLists(PacketList a, PacketList b)
            => (int)GetPacketOrder(a, b);

        private PacketOrder GetPacketOrder(PacketList left, PacketList right)
        {
            var limit = Math.Min(left.Items.Count, right.Items.Count);
            for (var i=0; i < limit; i++)
            {
                var leftItem = left.Items[i];
                var rightItem = right.Items[i];

                if (leftItem is PacketInteger && rightItem is PacketInteger)
                {
                    var l = ((PacketInteger)leftItem).Value;
                    var r = ((PacketInteger)rightItem).Value;

                    if (l < r)
                        return PacketOrder.Right;
                    else if (l > r)
                        return PacketOrder.Wrong;
                }
                else if (leftItem is PacketList && rightItem is PacketList)
                {
                    var l = (PacketList)leftItem;
                    var r = (PacketList)rightItem;

                    var result = GetPacketOrder(l, r);
                    if (result != PacketOrder.Equal)
                        return result;
                }
                else if (leftItem is PacketList)
                {
                    var l = (PacketList)leftItem;
                    var r = new PacketList((PacketInteger)rightItem);

                    var result = GetPacketOrder(l, r);
                    if (result != PacketOrder.Equal)
                        return result;
                }
                else
                {
                    var l = new PacketList((PacketInteger)leftItem);
                    var r = (PacketList)rightItem;

                    var result = GetPacketOrder(l, r);
                    if (result != PacketOrder.Equal)
                        return result;
                } 
                    
            }
            if (left.Items.Count < right.Items.Count)
                return PacketOrder.Right;
            else if (left.Items.Count > right.Items.Count)
                return PacketOrder.Wrong;
            else
                return PacketOrder.Equal;
        }

        private PacketList ParsePakcetList(int index)
            => ParsePakcetList(_lines[index]);

        private PacketList ParsePakcetList(string line)
        {
            (var result, _) = ParsePakcetList(line, 0);
            result.Name = line;
            return result;
        }

        private (PacketList subList, int endPos) ParsePakcetList(string line, int startPos)
        {
            if (line[startPos] != '[')
                throw new Exception("Parse error");

            var result = new PacketList();
            var currPos = startPos + 1;
            while (true)
            {
                if (line[currPos] == '[')
                {
                    (var subList, var endPos) = ParsePakcetList(line, currPos);
                    result.Items.Add(subList);
                    currPos = endPos+1;
                }
                else if (line[currPos] == ']')
                {
                    return (result, currPos);
                }
                else if (line[currPos] == ',')
                {
                    currPos++;
                }
                else
                {
                    var num = 0;
                    while (line[currPos] != ']' && line[currPos] != ',')
                    {
                        num *= 10;
                        num += line[currPos] - '0';
                        currPos++;
                    }
                    result.Items.Add(new PacketInteger(num));
                }    
            }
        }
    }
}