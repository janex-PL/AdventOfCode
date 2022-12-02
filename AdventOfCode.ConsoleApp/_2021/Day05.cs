using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.ConsoleApp._2021
{
    public class Day05
    {
        public static void Execute()
        {
            var data = DataProvider.GetData(2021, 5);
            Console.WriteLine(GetTotalDangerPoints(data.Split("\r\n"),true));
            Console.WriteLine(GetTotalDangerPoints(data.Split("\r\n"),false));
        }

        private static int GetTotalDangerPoints(string[] split, bool straightOnly)
        {
            var dict = new Dictionary<(int X, int Y), int>();
            foreach (var entry in split)
            {
                var cordsA = entry.Split(" -> ").First().Split(',').Select(int.Parse).ToList();
                var cordsB = entry.Split(" -> ").Last().Split(',').Select(int.Parse).ToList();

                (int x, int y) start = (cordsA.First(), cordsA.Last());
                (int x, int y) last = (cordsB.First(), cordsB.Last());
                
                var (x, y) = GetLineDirection(start, last);

                if (straightOnly && x != 0 && y != 0)
                    continue;

                var currentPos = start;

                while(true)
                {
                    if (!dict.TryGetValue(currentPos, out var _))
                        dict.Add(currentPos, 1);
                    else
                        dict[currentPos] += 1;
                    if (currentPos.x == last.x && currentPos.y == last.y)
                        break;
                    currentPos = (currentPos.x + x, currentPos.y + y);
                }
            }
            return dict.Count(x => x.Value >= 2);
        }



        private static (int x, int y) GetLineDirection((int x, int y) start, (int x, int y) last)
        {
            int x, y;
            if (start.x == last.x)
                x = 0;
            else if (start.x > last.x)
                x = -1;
            else
                x = 1;
            if(start.y == last.y)
                y = 0;
            else if (start.y > last.y)
                y = -1;
            else
                y = 1;

            return (x, y);
        }
    }
}
