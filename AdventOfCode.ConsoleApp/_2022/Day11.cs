using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace AdventOfCode.ConsoleApp._2022;
internal class Day11
{
    public static void Execute()
    {
        var data = DataProvider.GetData(2022, 11);

        Console.WriteLine($"Monkey business level after 20 rounds: {GetMonkeyBusinessLevel(data,20)}");
        Console.WriteLine($"Monkey business level after 10000: {GetMonkeyBusinessLevel(data,10000,true)}");
    }

    private static long GetMonkeyBusinessLevel(string data, int rounds, bool worryLevelManagment = false)
    {
        var monkeyInput = data.Split("\r\n\r\n");

        var monkeyInventories = new List<Queue<long>>();
        var monkeyOperations = new List<Func<long, long>>();
        var monkeyTests = new List<long>();
        var monkeyTestResultDestinations = new List<Tuple<int, int>>();
        var monkeyInspections = new List<long>();

        foreach (var monkey in monkeyInput)
        {
            var monkeyLines = monkey.Split("\r\n");
            monkeyInventories.Add(new Queue<long>(Regex.Matches(monkeyLines[1], @"\d+")
                .Select(x => long.Parse(x.Value))));

            var operationElements = monkeyLines[2].Trim(' ').Split(' ').ToArray();

            var operationSing = operationElements[4];

            Func<long, long> operation;

            if (operationElements.Count(x => x.Contains("old")) == 2)
            {
                operation = operationSing switch
                {
                    "+" => i => i + i,
                    "*" => i => i * i,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
            else
            {
                var number = long.Parse(operationElements[5]);
                operation = operationSing switch
                {
                    "+" => i => i + number,
                    "*" => i => i * number,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }

            monkeyOperations.Add(operation);

            var testNumber = long.Parse(Regex.Match(monkeyLines[3], @"\d+").Value);

            monkeyTests.Add(testNumber);

            var monkeyTrueDest = int.Parse(Regex.Match(monkeyLines[4], @"\d").Value);
            var monkeyFalseDest = int.Parse(Regex.Match(monkeyLines[5], @"\d").Value);

            monkeyTestResultDestinations.Add(new Tuple<int, int>(monkeyTrueDest, monkeyFalseDest));

            monkeyInspections.Add(0);
        }


         var factor = worryLevelManagment ? monkeyTests.Aggregate((t1, t2) => t1 * t2) : 3;

        for (long i = 0; i < rounds; i++)
        {
            for (int j = 0; j < monkeyInventories.Count; j++)
            {
                while (monkeyInventories[j].Any())
                {
                    var item = monkeyInventories[j].Dequeue();

                    var newValue = monkeyOperations[j].Invoke(item);

                    if(worryLevelManagment)
                        newValue %= factor;
                    else
                        newValue /= factor;

                    monkeyInventories[
                        newValue % monkeyTests[j] == 0
                            ? monkeyTestResultDestinations[j].Item1
                            : monkeyTestResultDestinations[j].Item2].Enqueue(newValue);

                    monkeyInspections[j] += 1;
                }
            }
        }


        return monkeyInspections.OrderByDescending(x => x).Take(2).Aggregate((x, y) => x * y);
    }
}
