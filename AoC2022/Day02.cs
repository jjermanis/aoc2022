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

        private readonly IDictionary<char, RPS> _MOVE_MAP =
            new Dictionary<char, RPS>()
            {
                { 'A', RPS.Rock },
                { 'B', RPS.Paper },
                { 'C', RPS.Scissors },
            };
        
        private readonly IDictionary<RPS, int> _PLAYER_SCORE =
            new Dictionary<RPS, int>()
            {
                { RPS.Rock, 1 },
                { RPS.Paper, 2 },
                { RPS.Scissors, 3 },
            };

        private readonly IDictionary<RPS, RPS> _WINNING_RESPONSE_MAP =
            new Dictionary<RPS, RPS>()
            {
                { RPS.Rock, RPS.Paper },
                { RPS.Paper, RPS.Scissors },
                { RPS.Scissors, RPS.Rock },
            };

        private readonly IDictionary<RPS, RPS> _LOSING_RESPONSE_MAP =
            new Dictionary<RPS, RPS>()
            {
                { RPS.Rock, RPS.Scissors },
                { RPS.Paper, RPS.Rock },
                { RPS.Scissors, RPS.Paper },
            };

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
            // In Plan A, X, Y, Z mean player plays Rock, Paper, Scissors 

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
                RPS playerMove;
                switch (line[2])
                {
                    case 'X':
                        playerMove = _LOSING_RESPONSE_MAP[opponentMove];
                        break;
                    case 'Y':
                        // Draw is also matching moves
                        playerMove = opponentMove;
                        break;
                    case 'Z':
                        playerMove = _WINNING_RESPONSE_MAP[opponentMove];
                        break;
                    default:
                        throw new Exception();
                }

                result += ScoreRound(opponentMove, playerMove);
            }
            return result;
        }

        private int ScoreRound(RPS opponent, RPS player)
        {
            var result = _PLAYER_SCORE[player];

            var delta = player - opponent;
            switch (delta)
            {
                case 0:
                    result += 3;
                    break;
                case 1:
                case -2:
                    result += 6;
                    break;
            }
            return result;
        }
    }
}