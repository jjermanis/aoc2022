using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC2022
{
    public class Day11 : DayBase, IDay
    {
        private class Monkey
        {
            public readonly int Id;
            public List<long> Items;
            public readonly char Operator;
            public readonly int Operand;
            public readonly int TestDivisor;
            public readonly int TestTargetIfTrue;
            public readonly int TestTargetIfFalse;
            public readonly bool IsDivideByThreeAtRoundEnd;

            public Monkey(
                List<string> notes,
                bool isDivideByThreeAtRoundEnd)
            {
                Id = int.Parse(notes[0][^3..^1]);

                var rawItems = notes[1][18..];
                Items = rawItems.Split(',').Select(long.Parse).ToList();

                var opsIndex = notes[2].IndexOf("= old ") + 6;
                var operation = notes[2][opsIndex..];
                Operator = operation[0];
                var rawOperand = operation[2..];
                if (rawOperand == "old")
                {
                    Operator = '^';
                    Operand = 2;
                }
                else
                    Operand = int.Parse(rawOperand);

                TestDivisor = int.Parse(notes[3][^2..^0]);
                TestTargetIfTrue = int.Parse(notes[4][^2..^0]);
                TestTargetIfFalse = int.Parse(notes[5][^2..^0]);

                IsDivideByThreeAtRoundEnd = isDivideByThreeAtRoundEnd;
            }

            public void InspectItems()
            {
                for (int i= 0; i < Items.Count; i++)
                {
                    long value = Items[i];
                    switch (Operator)
                    {
                        case '+':
                            value += Operand;
                            break;
                        case '*':
                            value *= Operand;
                            break;
                        case '^':
                            value *= value;
                            break;
                        default:
                            throw new Exception($"Unknown operator: {Operator}");
                    }
                    if (IsDivideByThreeAtRoundEnd)
                        value /= 3;
                    Items[i] = value;
                }
            }
        }
        private readonly List<string> _lines;

        public Day11(string filename)
            => _lines = TextFileLines(filename).ToList();

        public Day11() : this("Day11.txt")
        {
        }

        public void Do()
        {
            Console.WriteLine($"{nameof(MonkeyBusinessLevel)}: {MonkeyBusinessLevelShort()}");
            Console.WriteLine($"{nameof(MonkeyBusinessLevelLong)}: {MonkeyBusinessLevelLong()}");
        }

        public long MonkeyBusinessLevelShort()
        {
            // At end of each round, update each item priority by /= 3
            var monkeys = ParseMonkeys(true);

            return MonkeyBusinessLevel(monkeys, 20);
        }

        public long MonkeyBusinessLevelLong()
        {
            // At end of each round, DO NOT update each item priority by /= 3
            var monkeys = ParseMonkeys(false);

            return MonkeyBusinessLevel(monkeys, 10000);
        }

        private IList<Monkey> ParseMonkeys(bool isDivideByThreeAtRoundEnd)
        {
            var result = new List<Monkey>();

            for (var index = 0; index < _lines.Count + 1; index+= 7)
            {
                result.Add(new Monkey(
                    _lines.GetRange(index, 6), isDivideByThreeAtRoundEnd));
            }
            return result;
        }

        private long MonkeyBusinessLevel(
            IList<Monkey> monkeys,
            int totalRounds)
        {
            var inspectionCount = new long[monkeys.Count];
            long reducer = 1;
            foreach (var monkey in monkeys)
                reducer *= monkey.TestDivisor;
            bool isDivideByThreeAtRoundEnd = monkeys[0].IsDivideByThreeAtRoundEnd;

            for (var roundNum = 0; roundNum < totalRounds; roundNum++)
            {
                for (var currId = 0; currId < monkeys.Count; currId++)
                {
                    var currMonkey = monkeys[currId];
                    currMonkey.InspectItems();
                    foreach (var item in currMonkey.Items)
                    {
                        long reducedItem = isDivideByThreeAtRoundEnd ? item : item % reducer;

                        var destMonkey = reducedItem % currMonkey.TestDivisor == 0
                            ? currMonkey.TestTargetIfTrue
                            : currMonkey.TestTargetIfFalse;

                        monkeys[destMonkey].Items.Add(reducedItem);
                        inspectionCount[currId]++;
                    }
                    currMonkey.Items.Clear();
                }
            }
            Array.Sort(inspectionCount);
            Array.Reverse(inspectionCount);
            return inspectionCount[0] * inspectionCount[1];
        }
    }
}