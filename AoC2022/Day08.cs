using System;
using System.Collections.Generic;

namespace AoC2022
{
    public class Day08 : DayBase, IDay
    {
        private readonly char[,] _trees;
        private readonly int _width;
        private readonly int _height;

        public Day08(string filename)
        { 
             var lines = TextFileStringList(filename);
            (_trees, _width, _height) = GetTreeGrid(lines);
        }

        public Day08() : this("Day08.txt")
        {
        }

        public void Do()
        {
            Console.WriteLine($"{nameof(VisibleTreeCount)}: {VisibleTreeCount()}");
            Console.WriteLine($"{nameof(BestScenicScore)}: {BestScenicScore()}");
        }

        public int VisibleTreeCount()
        {
            var isVisible = new bool[_width, _height];

            // Check columns
            for (var x=0; x < _width; x++)
            {
                var tallest = ' ';
                for (var y=0; y < _height; y++)
                {
                    if (_trees[x,y] > tallest)
                    {
                        isVisible[x,y] = true;
                        tallest = _trees[x, y];
                    }
                }
                tallest = ' ';
                for (var y = _height-1; y >= 0; y--)
                {
                    if (_trees[x, y] > tallest)
                    {
                        isVisible[x, y] = true;
                        tallest = _trees[x, y];
                    }
                }
            }
            // Check rows
            for (var y = 0; y < _height; y++)
            {
                var tallest = ' ';
                for (var x = 0; x < _width; x++)
                {
                    if (_trees[x, y] > tallest)
                    {
                        isVisible[x, y] = true;
                        tallest = _trees[x, y];
                    }
                }
                tallest = ' ';
                for (var x = _width - 1; x >= 0; x--)
                {
                    if (_trees[x, y] > tallest)
                    {
                        isVisible[x, y] = true;
                        tallest = _trees[x, y];
                    }
                }
            }

            var result = 0;
            foreach (var elem in isVisible)
                if (elem)
                    result++;

            return result;
        }

        public int BestScenicScore()
        {
            var result = 0;
            for (var x = 1; x < _width-1; x++)
                for (var y = 1; y < _height-1; y++)
                {
                    var curr = ScenicScore(x, y);
                    if (curr > result)
                        result = curr;
                }
            return result;
        }

        private (char[,], int, int) GetTreeGrid(IList<string> lines)
        {
            var width = lines[0].Length;
            var height = lines.Count;
            var result = new char[width, height];

            for (int y = 0; y < height; y++)
            {
                var currLine = lines[y];
                for (int x = 0; x < width; x++)
                {
                    result[x,y] = currLine[x];
                }
            }
            return (result, width, height);
        }

        private int ScenicScore(
            int currX, 
            int currY)
        {
            var height = _trees[currX, currY];

            var rightScore = 0;
            for (var x = currX + 1; x < _width; x++)
            {
                rightScore++;
                if (_trees[x, currY] >= height)
                    break;
            }

            var leftScore = 0;
            for (var x = currX - 1; x >= 0; x--)
            {
                leftScore++;
                if (_trees[x, currY] >= height)
                    break;
            }

            var downScore = 0;
            for (var y = currY + 1; y < _height; y++)
            {
                downScore++;
                if (_trees[currX, y] >= height)
                    break;
            }

            var upScore = 0;
            for (var y = currY - 1; y >= 0; y--)
            {
                upScore++;
                if (_trees[currX, y] >= height)
                    break;
            }

            return leftScore * rightScore * upScore * downScore;
        }
    }
}