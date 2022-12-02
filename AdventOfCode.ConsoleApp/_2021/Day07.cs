using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.ConsoleApp._2021
{
    public class Day07
    {
        public static void Execute()
        {
            var data = DataProvider.GetData(2021, 7);
            var x = CheapestFuelPosition(data.Split(',').Select(int.Parse).ToList());
            var y = BetterCheapestFuelPosition(data.Split(',').Select(int.Parse).ToList());
            Console.WriteLine(x);
        }

        private static int CheapestFuelPosition(List<int> crabs)
        {
            return Enumerable.Range(0, crabs.Max()).Min(x => crabs.Select(y => Math.Abs(y - x)).Sum());
        }
        private static int BetterCheapestFuelPosition(List<int> crabs)
        {
            return Enumerable.Range(0, crabs.Max()).Min(x => crabs.Select(y => Enumerable.Range(1,Math.Abs(y-x)).Sum()).Sum());
        }
    }
}
