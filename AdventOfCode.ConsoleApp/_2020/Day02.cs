using System;
using System.Linq;

namespace AdventOfCode.ConsoleApp._2020
{
    public static class Day02
    {
        public static void Execute()
        {
            var input = DataProvider.GetData(2020, 2);
            Console.WriteLine(GetCorrectPasswordsCount(input));
            Console.WriteLine(GetCorrectPasswordLetterPosCount(input));
        }

        private static int GetCorrectPasswordsCount(string data)
        {
            var inputList = data.Split("\r\n").Select(x => x.Split(' ').ToList()).ToList();
            return (from entry in inputList
                    let letterCount = entry.First().Split('-').Select(int.Parse).ToList()
                    let letter = entry[1].Trim(':').First()
                    let inputCount = entry.Last().Count(x => x == letter)
                    where inputCount >= letterCount.First() && inputCount <= letterCount.Last()
                    select letterCount).Count();
        }

        private static int GetCorrectPasswordLetterPosCount(string data)
        {
            var inputList = data.Split("\r\n").Select(x => x.Split(' ').ToList()).ToList();

            return (from entry in inputList
                    let letterPos = entry.First().Split('-').Select(int.Parse).ToList()
                    let letter = entry[1].Trim(':').First()
                    let lettersToCheck = entry.Last()
                        .Where((_, i) => i == letterPos.First() - 1 || i == letterPos.Last() - 1)
                        .ToList()
                    where lettersToCheck.Count(x => x == letter) == 1
                    select letter).Count();
        }
    }
}