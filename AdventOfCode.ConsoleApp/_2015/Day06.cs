using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.ConsoleApp._2015;

public class Day06
{
    public static void Execute()
    {
        var data = DataProvider.GetData(2015, 6).Split("\r\n");
        Console.WriteLine(GetLightsCount(data));
        Console.WriteLine(GetLightsBrightness(data));
    }

    private static long GetLightsCount(string[] data)
    {
        var lights = new HashSet<(int,int)>();
        foreach (var entry in data)
        {
            var coords = Regex.Matches(entry, @"\d+").Select(x => int.Parse(x.Value)).ToArray();
            var (x1,y1) = (coords[0], coords[1]);
            var (x2,y2) = (coords[2], coords[3]);
            for(var i=y1; i<=y2; i++)
            for (var j = x1; j <= x2; j++)
            {
                switch (entry)
                {
                    case var x when x.Contains("turn on"):
                        if (!lights.TryGetValue((j, i), out _))
                            lights.Add((j, i));
                        break;
                    case var x when x.Contains("turn off"):
                        if (lights.TryGetValue((j, i), out _))
                            lights.Remove((j, i));
                        break;
                    case var x when x.Contains("toggle"):
                        if (!lights.TryGetValue((j, i), out _))
                            lights.Add((j, i));
                        else
                            lights.Remove((j, i));
                        break;
                }
            }
        }

        return lights.Count;
    }
    private static long GetLightsBrightness(string[] data)
    {
        var lights = new long[1000*1000];
        foreach (var entry in data)
        {
            var coords = Regex.Matches(entry, @"\d+").Select(x => int.Parse(x.Value)).ToArray();
            var (x1,y1) = (coords[0], coords[1]);
            var (x2,y2) = (coords[2], coords[3]);
            for(var i=y1; i<=y2; i++)
            for (var j = x1; j <= x2; j++)
            {
                switch (entry)
                {
                    case var x when x.Contains("turn on"):
                        lights[i*1000+ j] += 1;
                        break;
                    case var x when x.Contains("turn off"):
                        lights[i*1000 + j] = Math.Max(0, lights[i*1000 + j] - 1);
                        break;
                    case var x when x.Contains("toggle"):
                        lights[i*1000 + j] += 2;
                        break;
                }
            }
        }

        return lights.Sum();
    }
}