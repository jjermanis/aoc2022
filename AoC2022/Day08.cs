using System;
using System.Collections.Generic;

namespace AoC2022
{
    public class Day08 : DayBase, IDay
    {
        private readonly IList<string> _lines;

        public Day08(string filename)
            => _lines = TextFileStringList(filename);

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
            (var trees, var width, var height) = GetTreeGrid();
            var isVisible = new bool[width,height];

            // Check columns
            for (var x=0; x < width; x++)
            {
                var tallest = ' ';
                for (var y=0; y < height; y++)
                {
                    if (trees[x,y] > tallest)
                    {
                        isVisible[x,y] = true;
                        tallest = trees[x, y];
                    }
                }
                tallest = ' ';
                for (var y = height-1; y >= 0; y--)
                {
                    if (trees[x, y] > tallest)
                    {
                        isVisible[x, y] = true;
                        tallest = trees[x, y];
                    }
                }
            }
            // Check rows
            for (var y = 0; y < height; y++)
            {
                var tallest = ' ';
                for (var x = 0; x < width; x++)
                {
                    if (trees[x, y] > tallest)
                    {
                        isVisible[x, y] = true;
                        tallest = trees[x, y];
                    }
                }
                tallest = ' ';
                for (var x = width - 1; x >= 0; x--)
                {
                    if (trees[x, y] > tallest)
                    {
                        isVisible[x, y] = true;
                        tallest = trees[x, y];
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
            // 108192 is too low
            (var trees, var width, var height) = GetTreeGrid();
            var result = 0;
            for (var x = 1; x < width-1; x++)
                for (var y = 1; y < height-1; y++)
                {
                    var curr = ScenicScore(trees, width, height, x, y);
                    if (curr > result)
                        result = curr;
                }
            return result;
        }

        private (char[,], int, int) GetTreeGrid()
        {
            var width = _lines[0].Length;
            var height = _lines.Count;
            var result = new char[width, height];

            for (int y = 0; y < height; y++)
            {
                var currLine = _lines[y];
                for (int x = 0; x < width; x++)
                {
                    result[x,y] = currLine[x];
                }
            }
            return (result, width, height);
        }

        private int ScenicScore(
            char[,] trees, 
            int gridWidth,
            int gridHeight,
            int currX, 
            int currY)
        {
            var height = trees[currX, currY];

            var rightScore = 0;
            for (var x = currX + 1; x < gridWidth; x++)
            {
                rightScore++;
                if (trees[x, currY] >= height)
                    break;
            }

            var leftScore = 0;
            for (var x = currX - 1; x >= 0; x--)
            {
                leftScore++;
                if (trees[x, currY] >= height)
                    break;
            }

            var downScore = 0;
            for (var y = currY + 1; y < gridHeight; y++)
            {
                downScore++;
                if (trees[currX, y] >= height)
                    break;
            }

            var upScore = 0;
            for (var y = currY - 1; y >= 0; y--)
            {
                upScore++;
                if (trees[currX, y] >= height)
                    break;
            }

            return leftScore * rightScore * upScore * downScore;
        }
    }
}