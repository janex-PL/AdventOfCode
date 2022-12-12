using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.ConsoleApp._2022;
internal class Day10
{
    public static void Execute()
    {
        var data = DataProvider.GetData(2022, 10);
        Console.WriteLine($"Signal strength sum: {GetSignalStrengthSum(data)}");
        DisplayCrt(data);
    }

    private static int GetSignalStrengthSum(string data)
    {
        var instructions = data.Split("\r\n").ToList();

        var result = new List<int>();

        var registry = 1;
        var cycles = 0;
        foreach (var instruction in instructions)
        {
            var cycleCount = instruction.StartsWith("noop") ? 1 : 2;
            while (cycleCount > 0)
            {
                cycles++;
                cycleCount--;
                if (cycles % 40 == 20)
                    result.Add(registry);
            }

            if (instruction.StartsWith("addx"))
                registry += int.Parse(instruction.Split(' ').Last());
        }

        return result.Select((x, i) => x * (20 + i * 40)).Sum();
    }

    private static void DisplayCrt(string data)
    {
        var instructions = data.Split("\r\n").ToList();

        var screen = new List<char>();

        var registry = 1;
        var cycles = 0;
        foreach (var instruction in instructions)
        {
            var cycleCount = instruction.StartsWith("noop") ? 1 : 2;
            while (cycleCount > 0)
            {
                cycles++;
                cycleCount--;
                screen.Add(Math.Abs(registry-(cycles-1)%40) <=1 ? '#' : '.');
            }

            if (instruction.StartsWith("addx"))
                registry += int.Parse(instruction.Split(' ').Last());
        }

        Console.WriteLine(string.Join('\n',screen.Chunk(40).Select(x => string.Concat(x))));
    }
}
