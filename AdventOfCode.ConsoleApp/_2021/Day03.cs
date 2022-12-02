using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.ConsoleApp._2021
{
    public class Day03
    {
        public static void Execute()
        {
            var data = DataProvider.GetData(2021, 3).Split("\r\n");
            var result = GetSubmarineParameters(data);
            Console.WriteLine($"Gamma: {result[0]}, Epsilon: {result[1]}, Result: {result[0] * result[1]}");
            result = GetSubmarineLifeParameters(data);
            Console.WriteLine($"Oxygen: {result[0]}, Co2: {result[1]}, Result: {result[0] * result[1]}");
        }

        private static long[] GetSubmarineParameters(string[] data)
        {
            var result = Enumerable.Repeat(0, data.First().Length).ToList();

            foreach (var entry in data)
            {
                for (int i = 0; i < entry.Length; i++)
                {
                    if (entry[i] == '0')
                        result[i] -= 1;
                    else
                        result[i] += 1;
                }
            }

            var gammaRate = Convert.ToInt64( new string(result.Select(x => x < 0 ? '0' : '1').ToArray()),2);
            var epsilonRate = Convert.ToInt64(new string(result.Select(x => x < 0 ? '1' : '0').ToArray()),2);

            return new[] { gammaRate, epsilonRate };
        }

        private static long[] GetSubmarineLifeParameters(string[] data)
        {
            var oxygenResult = new List<char>();

            var oxygenData = data.ToList();
            for (int i = 0; i < data.First().Length; i++)
            {
                oxygenResult.Add(oxygenData.Count(x => x[i] == '0') > oxygenData.Count/2 ? '0' : '1');
                oxygenData = oxygenData.Where(x => x[i] == oxygenResult[i]).ToList();
                if (oxygenData.Count == 1)
                    break;
            }

            var co2Result = new List<char>();


            var co2Data = data.ToList();
            for (int i = 0; i < data.First().Length; i++)
            {
                co2Result.Add(co2Data.Count(x => x[i] == '0') > co2Data.Count/2 ? '1' : '0');
                co2Data = co2Data.Where(x => x[i] == co2Result[i]).ToList();
                if (co2Data.Count == 1)
                    break;
            }

            var oxygen = Convert.ToInt64(new string(oxygenData.First()), 2);
            var co2 = Convert.ToInt64(new string(co2Data.First()), 2);

            return new[] { oxygen, co2 };
        }
    }
}
