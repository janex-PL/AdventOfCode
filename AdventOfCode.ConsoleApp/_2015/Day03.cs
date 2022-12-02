using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.ConsoleApp._2015;

public class Day03
{
    private static readonly Dictionary<char, (int X, int Y)> Directions = new()
    {
        { '^', (0, 1) },
        { '>', (1, 0) },
        { '<', (-1, 0) },
        { 'v', (0, -1) },
    };

    public static void Execute()
    {
        var data = DataProvider.GetData(2015, 3);
        Console.WriteLine(GetHouses(data).Count);
        Console.WriteLine(GetHousesForTwoSanta(data).Count);
    }

    private static List<(int X, int Y)> GetHouses(string route)
    {
        var houses = new List<(int X, int Y)>() { (0, 0) };
        var currentPos = (X : 0, Y : 0);
        foreach (var direction in route)
        {
            (int X, int Y) move;

            if (Directions.TryGetValue(direction, out var value))
                move = value;
            else
                continue;

            currentPos = (currentPos.X+move.X, currentPos.Y+move.Y);
            if(!houses.Any(house => house.X == currentPos.X && house.Y == currentPos.Y))
                houses.Add(currentPos);
        }

        return houses;
    }

    private static List<(int X, int Y)> GetHousesForTwoSanta(string data)
    {
        var routes = new List<string>()
        {
            new(data.Where((_, i) => i % 2 == 0).ToArray()),
            new(data.Where((_, i) => i % 2 != 0).ToArray()),
        };
        var result = new List<(int X, int Y)>();
        routes.ForEach(x =>
        {
            result.AddRange(GetHouses(x));
        });
        return result.Distinct().ToList();
    }
}