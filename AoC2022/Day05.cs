using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC2022
{
    public class Day05 : DayBase, IDay
    {
        private struct ProcedureStep
        {
            public readonly int Count;
            public readonly int Source;
            public readonly int Destination;

            public ProcedureStep(string text)
            {
                Count = int.Parse(text[5..7]);
                Source = text[^6] - '0';
                Destination = text[^1] - '0';
            }
        }

        private delegate void StepImpStyle(
            List<Stack<char>> stacks,
            ProcedureStep step);

        private readonly IList<string> _lines;


        public Day05(string filename)
            => _lines = TextFileStringList(filename);

        public Day05() : this("Day05.txt")
        {
        }

        public void Do()
        {
            Console.WriteLine($"{nameof(SingleCrateSummary)}: {SingleCrateSummary()}");
            Console.WriteLine($"{nameof(MultiCrateSummary)}: {MultiCrateSummary()}");
        }

        public string SingleCrateSummary()
            => ExecuteProcedure(StepSingleCrateStyle);

        public string MultiCrateSummary()
            => ExecuteProcedure(StepMultiCrateStyle);

        private string ExecuteProcedure(StepImpStyle StepStyle)
        {
            (var stacks, var procedure) = ParseInput();
            foreach (var step in procedure)
                StepStyle(stacks, step);
            return DesiredCratesSummary(stacks);
        }

        private (List<Stack<char>>, List<ProcedureStep>) ParseInput()
        {
            int columnsLine;
            for (columnsLine = 0; columnsLine < _lines.Count; columnsLine++)
                if (_lines[columnsLine][1] == '1')
                    break;

            // Parse stacks
            var maxColumn = _lines[columnsLine][^2] - '0';
            var stacks = new List<Stack<char>>();
            for (var i = 0; i <= maxColumn; i++)
                stacks.Add(new Stack<char>());
            for (var y = columnsLine - 1; y >= 0; y--)
            {
                var currLine = _lines[y];
                for (var x = 1; x <= maxColumn; x++)
                {
                    var curr = currLine[x * 4 - 3];
                    if (curr != ' ')
                        stacks[x].Push(curr);
                }
            }

            // Parse procedure steps
            var procedure = new List<ProcedureStep>();
            for (var i = columnsLine + 2; i < _lines.Count; i++)
                procedure.Add(new ProcedureStep(_lines[i]));

            return (stacks, procedure);
        }
        private void StepSingleCrateStyle(
            List<Stack<char>> stacks,
            ProcedureStep step)
        {
            for (var i = 0; i < step.Count; i++)
            {
                var temp = stacks[step.Source].Pop();
                stacks[step.Destination].Push(temp);
            }
        }

        private void StepMultiCrateStyle(
            List<Stack<char>> stacks,
            ProcedureStep step)
        {
            var tempStack = new Stack<char>();
            for (var i = 0; i < step.Count; i++)
            {
                var temp = stacks[step.Source].Pop();
                tempStack.Push(temp);
            }
            foreach (var c in tempStack)
            {
                stacks[step.Destination].Push(c);
            }
        }

        private string DesiredCratesSummary(List<Stack<char>> stacks)
        {
            var result = "";
            for (int i=1; i < stacks.Count; i++)
                result += stacks[i].ElementAt(0);

            return result;
        }
    }
}