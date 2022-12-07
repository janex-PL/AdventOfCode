using System;
using System.Linq;

namespace AdventOfCode.ConsoleApp._2022;
public class Day06
{
    public static void Execute()
    {
        var data = DataProvider.GetData(2022, 6);
        Console.WriteLine($"First marker sized 4 after {GetCharCountForFirstMarker(data,4)}");
        Console.WriteLine($"First marker sized 14 after {GetCharCountForFirstMarker(data,14)}");
    }

    private static int GetCharCountForFirstMarker(string data, int markerSize)
    {
        for (var i = 0; i + markerSize < data.Length; i++)
        {
            if(data.Skip(i).Take(markerSize).Distinct().Count() == markerSize)
                return i+markerSize;
        }

        return -1;
    }
}
