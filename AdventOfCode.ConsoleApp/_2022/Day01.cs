using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.ConsoleApp._2022;
internal class Day01
{
    public static void Execute()
    {
        var data = DataProvider.GetData(2022, 1);
        Console.WriteLine("Advent of Code 2022 / Day 1");
        Console.WriteLine($"Most calories: {GetMostCalories(data)}");
        Console.WriteLine($"Most calories from top 3: {GetMostCaloriesFromTop3(data)}");
    }

    private static int GetMostCalories(string data) => GetCaloriesList(data).Max();
    private static int GetMostCaloriesFromTop3(string data) => GetCaloriesList(data).OrderByDescending(x => x).Take(3).Sum();
    private static IEnumerable<int> GetCaloriesList(string data) =>
        data.Split("\r\n\r\n").Select(x => x.Split("\r\n").Select(int.Parse).Sum());
}
