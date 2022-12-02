using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.ConsoleApp._2021
{
    public class Day11
    {
        public static void Execute()
        {
            var data = DataProvider.GetData(2021, 11).Split("\r\n");
            Console.WriteLine(GetTotalFlashes(data));
            Console.WriteLine(GetAllFlashStep(data));
        }

        private static int GetAllFlashStep(string[] data)
        {
            var octopusMap = string.Concat(data).Select(x => new Octopus(x)).ToArray();
            for (int i = 0; i < int.MaxValue; i++)
            {
                IncreaseEnergy(octopusMap);
                while (octopusMap.Any(x => x.Energy > 9))
                {
                    octopusMap.Select((x,i) => (x,i)).Where(item => item.x.Energy > 9).ToList().ForEach(item =>
                    {
                        if(!item.x.DidFlash)
                        {
                            Flash(octopusMap, item.i/10, item.i%10);
                        }
                    });
                }

                if (octopusMap.All(x => x.DidFlash))
                    return i;
                foreach (var octopus in octopusMap)
                {
                    octopus.DidFlash = false;
                }
            }


            return 0;
        }


        private static int GetTotalFlashes(string[] data)
        {
            var octopusMap = string.Concat(data).Select(x => new Octopus(x)).ToArray();
            var flashCount = 0;
            for (int i = 0; i < 100; i++)
            {
                IncreaseEnergy(octopusMap);
                while (octopusMap.Any(x => x.Energy > 9))
                {
                    octopusMap.Select((x,j) => (x,i: j)).Where(item => item.x.Energy > 9).ToList().ForEach(item =>
                    {
                        if(!item.x.DidFlash)
                        {
                            Flash(octopusMap, item.i/10, item.i%10);
                            flashCount++;
                        }
                    });
                }
                foreach (var octopus in octopusMap)
                {
                    octopus.DidFlash = false;
                }
            }


            return flashCount;
        }

        private static void DisplayMap(Octopus[] map)
        {
            for (int i = 0; i < 10; i++)
            {
                for(int j = 0;j< 10; j++)
                    Console.Write(map[i*10+j].Energy);
                Console.WriteLine();
            }
        }
        private static void IncreaseEnergy(Octopus[] octopusArray)
        {
            for(int i=0; i < 10; i++)
                for (int j = 0; j < 10; j++)
                    octopusArray[i * 10 + j].Energy += 1;
        }

        private static readonly List<(int i, int j)> DirectionsFilter = new()
        {
            (1, 1), (1, 0), (1, -1), (0, 1), (0, -1), (-1, 1), (-1, 0), (-1, -1)
        };

        private static void Flash(Octopus[] map, int i, int j)
        {
            map[i * 10 + j].Energy = 0;
            map[i * 10 + j].DidFlash = true;
            foreach (var dir in DirectionsFilter)
            {
                var newI = i + dir.i;
                var newJ = j + dir.j;
                if (newI >= 0 && newJ >= 0 && newI < 10 && newJ < 10 && !map[newI * 10 + newJ].DidFlash)
                    map[newI * 10 + newJ].Energy += 1;
            }
        }
    }

    public class Octopus
    {
        public int Energy { get; set; }
        public bool DidFlash { get; set; }

        public Octopus(char value)
        {
            Energy = int.Parse(value.ToString());
        }
    }
}
