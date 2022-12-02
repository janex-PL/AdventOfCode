using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.ConsoleApp._2021
{
    public class Day06
    {
        public static void Execute()
        {
            var data = DataProvider.GetData(2021, 6).Split(',').Select(int.Parse).ToArray();
            Console.WriteLine(GetLanternfishCount(data));
        }

        private static long GetLanternfishCount(IEnumerable<int> data)
        {
            var fishes = data.ToList();
            var fishDayColony = Enumerable.Repeat(0L, 9).ToList();
            foreach (var fish in fishes)
            {
                fishDayColony[fish] += 1;
            }

            for (int i = 0; i < 256; i++)
            {
                var newFishDayColony = Enumerable.Repeat(0l, 9).ToList();
                for (int j = 8; j >= 1; j--)
                {
                    newFishDayColony[j - 1] = fishDayColony[j];
                }

                newFishDayColony[8] += fishDayColony[0];
                newFishDayColony[6] += fishDayColony[0];
                fishDayColony = newFishDayColony;

            }

            return fishDayColony.Sum();
        }
    }
}
