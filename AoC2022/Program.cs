using System;

namespace AoC2022
{
    internal class Program
    {
        private static void Main()
        {
            int start = Environment.TickCount;

            new Day01().Do();

            Console.WriteLine($"Time: {Environment.TickCount - start} ms");
        }
    }
}