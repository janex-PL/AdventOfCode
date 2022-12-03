using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.ConsoleApp._2022;

public class Day03
{
    public static void Execute()
    {
        var data = DataProvider.GetData(2022, 3);
        Console.WriteLine("Advent of Code 2022 / Day 3");
        Console.WriteLine($"Total priority: {GetTotalPriority(data)}");
        Console.WriteLine($"Total priority badge: {GetPriorityBadgeSum(data)}");
    }

    private static int GetTotalPriority(string data)
    {
        var rucksacks = data.Split("\n").Select(x => (x[..(x.Length / 2)], x.Substring(x.Length / 2, x.Length / 2)));
        
        return rucksacks.Select(x => x.Item1.Intersect(x.Item2).First()).Select(GetPriorityValue).ToList().Sum();
    }

    private static int GetPriorityBadgeSum(string data)
    {
        var rucksacks = data.Split("\n");
        var priorities = new List<int>();
        for (var i = 0; i < rucksacks.Length/3; i++)
        {
            var groupRucksacks = rucksacks.Skip(i * 3).Take(3).ToList();

            var priority = groupRucksacks[0].Intersect(groupRucksacks[1]).Intersect(groupRucksacks[2]).First();
            
            priorities.Add(GetPriorityValue(priority));
        }

        return priorities.Sum();
    }
    private static int GetPriorityValue(char x) => char.IsLower(x) ? x + 1 - 'a' : x + 27 - 'A';
}