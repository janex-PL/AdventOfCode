using System;
using System.Linq;

namespace AdventOfCode.ConsoleApp._2015;

public class Day02
{
    public static void Execute()
    {
        var data = DataProvider.GetData(2015, 2);
        Console.WriteLine("Advent of Code 2015 / Day 1");
        Console.WriteLine("Wrapping paper: " + GetWrappingPaper(data));
        Console.WriteLine("Ribbon: " + GetRibbon(data));
    }

    private static long GetWrappingPaper(string data)
    {
        return data.Split("\r\n").Select(x => x.Split('x').Select(long.Parse).OrderBy(y => y).ToList())
            .Select(entry => new[] { entry[0] * entry[1], entry[1] * entry[2], entry[0] * entry[2] }.OrderBy(x => x))
            .Select(area => area.First() + area.Select(x => 2 * x).Sum()).Sum();
    }

    private static long GetRibbon(string data)
    {
        return data.Split("\r\n").Select(x => x.Split('x').Select(long.Parse).OrderBy(y => y).ToList())
            .Select(entry => new[] { 2 * entry[0], 2 * entry[1], entry[0] * entry[1] * entry[2] }.Sum()).Sum();
    }
}