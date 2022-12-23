using System;
using System.Collections.Generic;

namespace AoC2022
{
    public class Day21 : DayBase, IDay
    {
        // TODO this is already fairly fast... but would be faster if HumanIntervention()
        // tried to "simplify" the equation first. This can be done by evaluating anything
        // that does use HUMN value

        private const string HUMN = "humn";

        private readonly IEnumerable<string> _lines;

        public Day21(string filename)
            => _lines = TextFileLines(filename);

        public Day21() : this("Day21.txt")
        {
        }

        public void Do()
        {
            Console.WriteLine($"{nameof(MonkeyComputation)}: {MonkeyComputation()}");
            Console.WriteLine($"{nameof(HumanIntervention)}: {HumanIntervention()}");
        }

        public long MonkeyComputation()
        {
            (var values, var ops) = ParseFile();
            var waiting = new Queue<(string name, string operand1, string operand2, char optr)>(ops);
            while (waiting.Count > 0)
            {
                var currEquation = waiting.Dequeue();
                var currResult = ComputeEquation(currEquation, values, true);
                if (currResult.HasValue)
                    values[currEquation.name] = currResult.Value;
                else
                    waiting.Enqueue(currEquation);
            }
            return values["root"];
        }

        public long HumanIntervention()
        {
            (var baseValues, var baseOps) = ParseFile();
            long minValue = -1_000_000_000_000_000L;
            long maxValue = 1_000_000_000_000_000L;

            // Figure out what effect changing the value for HUMN has on the result
            (var l1, var r1) = Guess(baseValues, baseOps, 1, true);
            (var l2, var r2) = Guess(baseValues, baseOps, 100, true);
            bool isAscending = (r2 - r1) < (l2 - l1);

            // Try different values for HUMN
            while (true)
            {
                var currGuess = (minValue + maxValue) / 2;
                (var left, var right) = Guess(baseValues, baseOps, currGuess, true);
                if (left == right)
                    break; 
                else
                    (minValue, maxValue) = AdjustRange(
                        minValue, maxValue, 
                        currGuess,
                        left, right,
                        isAscending);
            }

            // There is (likely) a range remain. Try each, without division rounding
            for (long x = minValue; x <= maxValue; x++)
            {
                try
                {
                    (var left, var right) = Guess(baseValues, baseOps, x, true);
                    if (left == right)
                        return x;
                }
                catch (Exception)
                {

                }
            }
            throw new Exception("No match found");
        }

        private (long minValue, long maxValue) AdjustRange(
            long minValue, long maxValue,
            long currGuess,
            long left, long right, 
            bool isAscending)
        {
            if (isAscending)
            {
                left = -left;
                right = -right;
            }
            if (left > right)
                minValue = currGuess + 1;
            else
                maxValue = currGuess - 1;
            return (minValue, maxValue);
        }

        private (long op1, long op2) Guess(
            IDictionary<string, long> startingValues, 
            IList<(string name, string operand1, string operand2, char optr)> startingOps,
            long guess,
            bool allowRounding)
        {
            var currValues = new Dictionary<string, long>(startingValues);
            var currOps = new List<(string name, string operand1, string operand2, char optr)>(startingOps);

            currValues[HUMN] = guess;
            var waiting = new Queue<(string name, string operand1, string operand2, char optr)>(currOps);

            while (true)
            {
                var currEquation = waiting.Dequeue();
                if (currEquation.name != "root")
                {
                    var currResult = ComputeEquation(currEquation, currValues, allowRounding);
                    if (currResult.HasValue)
                        currValues[currEquation.name] = currResult.Value;
                    else
                        waiting.Enqueue(currEquation);
                }
                else
                {
                    if (currValues.ContainsKey(currEquation.operand1) &&
                        currValues.ContainsKey(currEquation.operand2))
                    {
                        return (currValues[currEquation.operand1], currValues[currEquation.operand2]);
                    }
                    else
                        waiting.Enqueue(currEquation);
                }
            }
        }


        private long? ComputeEquation(
            (string name, string operand1, string operand2, char optr) eq,
            IDictionary<string, long> values,
            bool allowRounding)
        {
            if (values.ContainsKey(eq.operand1) && values.ContainsKey(eq.operand2))
            {
                var v1 = values[eq.operand1];
                var v2 = values[eq.operand2];

                if (!allowRounding && v1 % v2 != 0)
                    throw new Exception();

                var newValue = eq.optr switch
                {
                    '+' => v1 + v2,
                    '-' => v1 - v2,
                    '*' => v1 * v2,
                    '/' => v1 / v2,
                    _ => throw new Exception()
                };
                return newValue;
            }
            else
                return null;
        }

        private (IDictionary<string, long> values, IList<(string name, string operand1, string operand2, char optr)> pending) ParseFile()
        {
            var values = new Dictionary<string, long>();
            var pending = new List<(string name, string operand1, string operand2, char optr)>();

            foreach (var line in _lines)
            {
                var name = line[0..4];
                var value = line[6..];
                if (Char.IsDigit(value[0]))
                    values[name] = long.Parse(value);
                else
                {
                    var elements = value.Split(' ');
                    pending.Add((name, elements[0], elements[2], elements[1][0]));
                }
            }
            return (values, pending);
        }
    }
}