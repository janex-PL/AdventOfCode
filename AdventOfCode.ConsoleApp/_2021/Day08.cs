using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.ConsoleApp._2021
{
    public class Day08
    {
        public static void Execute()
        {
            var data = DataProvider.GetData(2021, 8).Split("\r\n");
            Console.WriteLine(GetUniqueDigitsCount(data));
            Console.WriteLine(GetOutputValueSum(data));
        }

        private static long GetUniqueDigitsCount(string[] data)
        {
            return data.Select(line => line.Split('|').Last().Trim().Split(' ')).Select(numbers =>
                numbers.Count(x => x.Length == 2 || x.Length == 3 || x.Length == 4 || x.Length == 7)).Sum();
        }

        private static long GetOutputValueSum(string[] data)
        {
            var sum = 0L;
            foreach (var line in data)
            {
                var connections = new Dictionary<char, char>();
                var digitsConfiguration = line.Split('|').First().Trim().Split(' ');

                var numberOne = digitsConfiguration.First(x => x.Length == 2);
                var numberSeven = digitsConfiguration.First(x => x.Length == 3);
                var numberFour = digitsConfiguration.First(x => x.Length == 4);
                var numberEight = digitsConfiguration.First(x => x.Length == 7);
                
                connections.Add(numberSeven.First(x => !numberOne.Contains(x)),'a');

                var connectionCResult = numberOne.OrderBy(x => digitsConfiguration.Count(y => y.Contains(x))).First();
                var connectionFResult = numberOne.OrderBy(x => digitsConfiguration.Count(y => y.Contains(x))).Last();

                connections.Add(connectionCResult,'c');
                connections.Add(connectionFResult,'f');

                var fiveSegmentedNumbers = digitsConfiguration.Where(x => x.Length == 5).ToArray();

                var connectionDResult = numberFour.Except(numberOne)
                    .First(x => fiveSegmentedNumbers.Count(y => y.Contains(x)) == 3);
                var connectionBResult = numberFour.Except(numberOne + connectionDResult).First();

                connections.Add(connectionDResult,'d');
                connections.Add(connectionBResult,'b');

                var remainingSegments = numberEight.Except(string.Concat(connections.Select(x => x.Key))).ToArray();
                var connectionGResult =
                    remainingSegments.First(x => fiveSegmentedNumbers.Count(y => y.Contains(x)) == 3);
                var connectionEResult = remainingSegments.Except(new []{connectionGResult}).First();

                connections.Add(connectionGResult,'g');
                connections.Add(connectionEResult,'e');

                var outputDigits = line.Split('|').Last().Trim().Split(' ')
                    .Select(x => string.Concat(x.Select(y => connections[y]).OrderBy(y => y))).Select(x => SegmentedDisplayDigits[x]);

                var outputNumber = int.Parse(string.Concat(outputDigits));
                sum += outputNumber;
            }

            return sum;
        }

        private static readonly Dictionary<string, string> SegmentedDisplayDigits = new()
        {
            { "abcefg", "0" },
            { "cf", "1" },
            { "acdeg", "2" },
            { "acdfg", "3" },
            { "bcdf", "4" },
            { "abdfg", "5" },
            { "abdefg", "6" },
            { "acf", "7" },
            { "abcdefg", "8" },
            { "abcdfg", "9" },
        };
    }
}
