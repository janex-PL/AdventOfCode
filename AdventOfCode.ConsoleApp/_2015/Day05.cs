using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;


namespace AdventOfCode.ConsoleApp._2015;

public class Day05
{
    public static void Execute()
    {
        var data = DataProvider.GetData(2015, 5).Split("\r\n");
        Console.WriteLine(GetNiceStrings(data).Count);
        Console.WriteLine(GetEvenMoreNiceStrings(data).Count);
    }


    private static readonly string[] ForbiddenStrings = new[] { "ab", "cd", "pq", "xy" };
    private static readonly string Vowels = "aeiou";
    private static List<string> GetNiceStrings(string[] data)
    {
        return data.Where(word => !ForbiddenStrings.Any(word.Contains) &&
                                  word.Count(letter => Vowels.Contains(letter)) >= 3 &&
                                  word.Zip(word.Skip(1))
                                      .Any(pair => pair.First == pair.Second))
            .ToList();
    }

    private static List<string> GetEvenMoreNiceStrings(string[] data)
    {
        var wordsWithDistinctPairs =
            data.Select(word => (Word: word, Pairs: word.Zip(word.Skip(1)).Distinct().ToList())).ToList();

        var wordsWithRepeatedPairs = wordsWithDistinctPairs.Select(entry => (entry.Word,
                entry.Pairs.Select(x => Regex.Matches(entry.Word, x.First.ToString() + x.Second))
                    .Where(x => x.Count > 1).Select(x => x.Select(y => y.Index).ToList()).ToList()))
            .Where(x => x.Item2.Any(y => y.Any())).ToList();

        var wordsWithMatchingPairIndexes = wordsWithRepeatedPairs.Select(entry => (entry.Word,
            entry.Item2.Select(x => x.Zip(x.Skip(1), (a, b) => Math.Abs(a - b)).ToList()).ToList())).ToList();

        var pairResult = wordsWithMatchingPairIndexes.Where(x => x.Item2.Any(y => y.Any(z => z > 1))).ToList();

        var wordsWithThrees = data.Select(word => (word, word.Zip(word.Skip(1), word.Skip(2))))
            .Where(x => x.Item2.Any(y => y.First == y.Third)).ToList();

        return pairResult.Select(x => x.Word).Intersect(wordsWithThrees.Select(x => x.word)).ToList();
    }
}