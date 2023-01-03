using System;
using System.Collections.Generic;

namespace AoC2022
{
    public class Day02 : DayBase, IDay
    {
        private readonly IEnumerable<string> _lines;

        private enum RPS
        {
            Rock = 0,
            Paper = 1,
            Scissors = 2
        }

        public Day02(string filename)
            => _lines = TextFileLines(filename);

        public Day02() : this("Day02.txt")
        {
        }

        public void Do()
        {
            Console.WriteLine($"{nameof(PlanAScore)}: {PlanAScore()}");
            Console.WriteLine($"{nameof(PlanBScore)}: {PlanBScore()}");
        }

        public int PlanAScore()
        {
            // A, B, C always means opponent plays Rock, Paper, Scissors
            // In Plan A, X, Y, Z means player plays Rock, Paper, Scissors 

            var result = 0;
            foreach (var line in _lines)
            {
                RPS opponentMove = (RPS)(line[0] - 'A');
                RPS playerMove = (RPS)(line[2] - 'X');

                result += ScoreRound(opponentMove, playerMove);
            }
            return result;
        }

        public int PlanBScore()
        {
            // A, B, C always means opponent plays Rock, Paper, Scissors 
            // In Plan B, X, Y, Z mean player plays to lose, draw, or win

            var result = 0;
            foreach (var line in _lines)
            {
                RPS opponentMove = (RPS)(line[0] - 'A');
                RPS playerMove = line[2] switch
                {
                    'X' => (RPS)(((int)(opponentMove + 2)) % 3),
                    'Y' => opponentMove,
                    'Z' => (RPS)(((int)(opponentMove + 1)) % 3),
                    _ => throw new Exception()
                };

                result += ScoreRound(opponentMove, playerMove);
            }
            return result;
        }

        private int ScoreRound(RPS opponent, RPS player)
        {
            var shapeScore = (int)player + 1;

            var delta = player - opponent;
            var outcomeScore = delta switch
            {
                0 => 3,
                1 => 6,
                -2 => 6,
                _ => 0
            };
            return shapeScore + outcomeScore;
        }
    }
}