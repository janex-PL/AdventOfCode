using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.ConsoleApp._2020
{
    public class Day03
    {
        public static void Execute()
        {
            var input = DataProvider.GetData(2020, 3);
            Console.WriteLine(GetTreeCount(input));
            Console.WriteLine(GetTreeCountMultiplier(input));
        }

        private static int GetTreeCount(string input)
        {
            var data = input.Split("\r\n").ToList();
            var lineLength = data.First().Length;
            var ctr = 0;
            for (var i = 0; i < data.Count; i++)
            {
                var xVal = (i * 3) % lineLength;
                if (data[i][xVal] == '#')
                    ctr++;
            }

            return ctr;
        }

        private static ulong GetTreeCountMultiplier(string input)
        {
            var data = input.Split("\r\n").ToList();
            var lineLength = data.First().Length;
            var stepList = new List<int>() { 1, 3, 5, 7 };
            var resultList = new List<ulong>() { 0, 0, 0, 0, 0 };
            for (var i = 0; i < data.Count; i++)
            {
                for (var j = 0; j < stepList.Count; j++)
                {
                    if (data[i][(i * stepList[j]) % lineLength] == '#')
                        resultList[j]++;
                    if (j == 0 && i % 2 != 0 && data[i][(i * stepList[j]) % lineLength] == '#')
                        resultList[4]++;
                }
            }

            return resultList[0] * resultList[1] * resultList[2] * resultList[3] * resultList[4];
        }
    }
}