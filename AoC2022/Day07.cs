using System;
using System.Collections.Generic;

namespace AoC2022
{
    public class Day07 : DayBase, IDay
    {
        private readonly IEnumerable<string> _lines;

        public Day07(string filename)
            => _lines = TextFileLines(filename);

        public Day07() : this("Day07.txt")
        {
        }

        public void Do()
        {
            Console.WriteLine($"{nameof(SmallDirectoryTotalSize)}: {SmallDirectoryTotalSize()}");
            Console.WriteLine($"{nameof(OptimalDirectoryDeletionSize)}: {OptimalDirectoryDeletionSize()}");
        }

        public int SmallDirectoryTotalSize()
        {
            var directorySizes = DirectorySizes();

            var result = 0;
            foreach (var size in directorySizes.Values)
                if (size <= 100000)
                    result += size;
            return result;
        }

        public int OptimalDirectoryDeletionSize()
        {
            var directorySizes = DirectorySizes();

            var totalSize = directorySizes["/"];
            var spaceNeeded = totalSize - 40000000;

            var result = totalSize;
            foreach (var size in directorySizes.Values)
                if (size >= spaceNeeded && size < result)
                        result = size;
            return result;
        }

        private IDictionary<string, int> DirectorySizes()
        {
            var currDirs = new List<string>();
            var directorySizes = new Dictionary<string, int>();

            foreach (var line in _lines)
            {
                if (line == "$ cd /")
                    continue;
                if (line == "$ cd ..")
                    currDirs.RemoveAt(currDirs.Count - 1);
                else if (line[0..4] == "$ cd")
                    currDirs.Add(line[5..]);
                else if (line[0] >= '0' && line[0] <= '9')
                {
                    var size = int.Parse(line.Split(' ')[0]);
                    foreach (var dir in DirectoryChain(currDirs))
                    {
                        directorySizes.TryGetValue(dir, out var currentCount);
                        directorySizes[dir] = currentCount + size;
                    }
                }
            }
            return directorySizes;
        }

        private IEnumerable<string> DirectoryChain(List<string> dirs)
        {
            yield return "/";
            string result = "";
            foreach (var dir in dirs)
            {
                result += "/" + dir;
                yield return result;
            }
        }
    }
}