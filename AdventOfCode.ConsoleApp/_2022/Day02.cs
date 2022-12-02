using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.ConsoleApp._2022;
internal class Day02
{
    public static void Execute()
    {
        var data = DataProvider.GetData(2022, 2);
        Console.WriteLine("Advent of Code 2022 / Day 2");
        Console.WriteLine($"Total score: {GetTotalScore(data)}");
        Console.WriteLine($"Total score by strategy: {GetTotalScoreByStrategy(data)}");
    }

    private static readonly Dictionary<char, char> WinningCombinations = new()
    {
        {'A','B'},
        {'B','C'},
        {'C','A'}
    };
    private static readonly Dictionary<char, char> LosingCombinations = new()
    {
        {'A','C'},
        {'B','A'},
        {'C','B'}
    };

    private static int GetTotalScoreByStrategy(string data)
    {
        var strategyDetails = TransformData(data);

        var gamesDetails = strategyDetails.Select(x => (x.Item1,
            x.Item2 switch
            {
                'A' => LosingCombinations[x.Item1],
                'C' => WinningCombinations[x.Item1],
                _ => x.Item1
            })).ToArray();

        return GetMatchesScore(gamesDetails);
    }

    private static int GetTotalScore(string data)
    {
        return GetMatchesScore(TransformData(data));
    }

    private static (char, char)[] TransformData(string data)
    {
        return data.Split("\r\n").Select(x => x.Split(" ")).Select(x => (x[0][0], (char)(x[1][0] - 'X' + 'A')))
            .ToArray();
    }

    private static int GetMatchesScore((char, char)[] gamesDetails)
    {
        var matchScore =
            gamesDetails.Select(x => x.Item1 == x.Item2 ? 3 : WinningCombinations[x.Item1] == x.Item2 ? 6 : 0).ToArray();

        var moveScore = gamesDetails.Select(x =>x.Item2 - 'A' + 1).ToArray();

        return matchScore.Sum() + moveScore.Sum();
    }
}
