using System;
using System.Linq;

namespace AdventOfCode.ConsoleApp._2022;

public class Day04
{
    public static void Execute()
    {
        var data = DataProvider.GetData(2022, 4);
        Console.WriteLine($"Total self included range: {GetTotalSelfRangeIncludes(data)}");
        Console.WriteLine($"Total overlaped range: {GetTotalOverlapedRanges(data)}");
    }

    private static int GetTotalSelfRangeIncludes(string data)
    {
        var pairs = data.Split('\n').Select(x => x.Split(','))
            .Select(x => (new RangePair(x[0]), new RangePair(x[1])));

        return pairs.Count(x =>
            (x.Item1.Min >= x.Item2.Min && x.Item1.Max <= x.Item2.Max) ||
            x.Item2.Min >= x.Item1.Min && x.Item2.Max <= x.Item1.Max);
    }
    
    private static int GetTotalOverlapedRanges(string data)
    {
        var pairs = data.Split('\n').Select(x => x.Split(','))
            .Select(x => (new RangePair(x[0]), new RangePair(x[1])));

        return pairs.Count(x =>
            Enumerable.Range(x.Item1.Min, x.Item1.Max - x.Item1.Min + 1)
                .Intersect(Enumerable.Range(x.Item2.Min, x.Item2.Max - x.Item2.Min + 1)).Any());

    }

    private class RangePair
    {
        public int Min { get; init; }
        public int Max { get; init; }

        public RangePair(string pair)
        {
            var numberPair = pair.Split('-');
            Min = int.Parse(numberPair.First());
            Max = int.Parse(numberPair.Last());
        }
    }
}