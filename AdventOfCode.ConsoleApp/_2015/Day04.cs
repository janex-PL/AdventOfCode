using System;
using System.Security.Cryptography;
using System.Text;

namespace AdventOfCode.ConsoleApp._2015;

public class Day04
{
    public static void Execute()
    {
        var data = DataProvider.GetData(2015, 4);
        GetMd5AppendedNumber(data);
    }

    private static int GetMd5AppendedNumber(string data)
    {
        var md5 = MD5.Create();
        var solvedFirst = false;
        var solvedSecond = false;
        var i = 0;
        while (!solvedFirst || !solvedSecond)
        {
            var input = Encoding.ASCII.GetBytes(data + i);
            var hash = md5.ComputeHash(input);
            if(hash[0] == 0 && hash[1] == 0 && hash[2] < 10)
            {
                Console.WriteLine("Part 1: " + i);
                solvedFirst= true;
            }
            if(hash[0] == 0 && hash[1] == 0 && hash[2] == 0)
            {
                Console.WriteLine("Part 2: " + i);
                solvedSecond= true;
            }
            i++;
        }

        return 0;
    }
}