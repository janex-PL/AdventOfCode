using System;
using System.Collections.Generic;

namespace AdventOfCode.ConsoleApp._2021
{
    public class Day02
    {
        public static void Execute()
        {
            var data = DataProvider.GetData(2021, 2).Split("\r\n");
            Console.WriteLine(GetMultipliedPosition(data));
            Console.WriteLine(GetMultipliedPositionWithAim(data));
        }

        private static long GetMultipliedPosition(IEnumerable<string> data)
        {
            var horizontal = 0;
            var depth = 0;
            foreach (var step in data)
            {
                var stepDetail = step.Split(' ');
                var unit = int.Parse(stepDetail[1]);
                switch (stepDetail[0])
                {
                    case "forward":
                        horizontal += unit;
                        break;
                    case "up":
                        depth -= unit;
                        break;
                    case "down":
                        depth += unit;
                        break;
                }
            }
            return horizontal * depth;
        }

        private static long GetMultipliedPositionWithAim(IEnumerable<string> data)
        {
            var horizontal = 0;
            var depth = 0;
            var aim = 0;
            foreach (var step in data)
            {
                var stepDetail = step.Split(' ');
                var unit = int.Parse(stepDetail[1]);
                switch (stepDetail[0])
                {
                    case "forward":
                        horizontal += unit;
                        depth += aim * unit;
                        break;
                    case "up":
                        aim -= unit;
                        break;
                    case "down":
                        aim += unit;
                        break;
                }
            }

            return horizontal * depth;
        }
    }
}
