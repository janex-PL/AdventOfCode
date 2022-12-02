using System;
using System.Linq;

namespace AdventOfCode.ConsoleApp._2015;

public class Day01 
{
    public static void Execute()
    {
        var data = DataProvider.GetData(2015, 1);
        Console.WriteLine("Advent of Code 2015 / Day 1");
        Console.WriteLine($"Result floor: {GetFloor(data)}");
        Console.WriteLine($"Entering basement at position: {GetBasementPosition(data)}");
    }

    private static int GetFloor(string input)
    {
        return input.Count(x => x == '(') - input.Count(x => x == ')');
    }

    private static int GetBasementPosition(string input)
    {
        var floor = 0;
        for (var i = 0; i < input.Length; i++)
        {
            floor -= input[i] == ')' ? 1 : -1;
            if (floor == -1)
                return i + 1;
        }
        return -1;
    }
}