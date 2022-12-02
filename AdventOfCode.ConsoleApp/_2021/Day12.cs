using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.ConsoleApp._2021
{
    public class Day12
    {
        public static void Execute()
        {
            var data = DataProvider.GetData(2021,12).Split("\r\n");
            Console.WriteLine(GetAllPaths(data));
        }

        private static int GetAllPaths(string[] data)
        {
            var caves = new List<Cave>();
            foreach (var line in data)
            {
                var caveNames = line.Split('-');

                var firstCave = caves.FirstOrDefault(x => x.Name == caveNames.First());
                if (firstCave is null)
                {
                    firstCave = new Cave(caveNames.First());
                    caves.Add(firstCave);
                }
                var secondCave = caves.FirstOrDefault(x => x.Name == caveNames.Last());
                if (secondCave is null)
                {
                    secondCave = new Cave(caveNames.Last());
                    caves.Add(secondCave);
                }
                firstCave.ConnectedCaves.Add(secondCave);
                secondCave.ConnectedCaves.Add(firstCave);
            }

            var result = GetCavePaths(caves, caves.First(x => x.Name == "start"), new List<Cave>());

            return result;
        }

        private static int GetCavePaths(List<Cave> caves, Cave current, List<Cave> list)
        {
            if (current.Name == "end")
            {
                return 1;
            }

            if (current.IsSmall && list.Any(x => x.Name == current.Name))
                return 0;
            var result = 0;
            list.Add(current);
            foreach (var cave in current.ConnectedCaves)
            {
                result += GetCavePaths(caves, cave, list);
            }

            return result;
        }
    }

    public class Cave
    {
        public string Name { get; }
        public bool IsSmall { get; }
        public List<Cave> ConnectedCaves { get; set; }
        public Cave(string name)
        {
            Name = name;
            IsSmall = !Regex.IsMatch(name, "[A-Z]");
            ConnectedCaves = new List<Cave>();
        }
    }
}
