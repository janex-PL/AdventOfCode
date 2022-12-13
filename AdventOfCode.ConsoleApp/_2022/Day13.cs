using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Nodes;

namespace AdventOfCode.ConsoleApp._2022;
internal class Day13
{
    public static void Execute()
    {
        var data = DataProvider.GetData(2022, 13);
        Console.WriteLine($"Right order count: {GetRightOrderCount(data)}");
        Console.WriteLine($"Decoder key: {GetDecoderKey(data)}");
    }

    private static int GetRightOrderCount(string data)
    {
        var pairs = data.Split("\r\n\r\n");

        var result = new List<int>();

        for (var index = 0; index < pairs.Length; index++)
        {
            var pair = pairs[index];
            var pairInputs = pair.Split("\r\n");

            var listA = JsonNode.Parse(pairInputs[0]);
            var listB = JsonNode.Parse(pairInputs[1]);

            var compareResult = IsRightOrder(listA.AsArray(), listB.AsArray());

            if(compareResult.HasValue && compareResult.Value)
                result.Add(index+1);
        }

        return result.Sum();
    }

    private static int GetDecoderKey(string data)
    {
        var input = data.Split("\r\n").Where(x => !string.IsNullOrEmpty(x)).Select(x => JsonNode.Parse(x)!.AsArray()).OrderBy(x => GetArrayKey(x.AsArray()))
            .ToList();

        var firstPacket = JsonNode.Parse("[[2]]").AsArray();
        var secondPacket = JsonNode.Parse("[[6]]").AsArray();

        input.Add(firstPacket);
        input.Add(secondPacket);

        var sorted = new List<JsonArray>();

        foreach (var arr in input)
        {
            var added = false;
            for (int i = 0; i < sorted.Count; i++)
            {
                var result = IsRightOrder(arr, sorted[i]);
                if(result.HasValue && result.Value)
                {
                    sorted.Insert(i,arr);
                    added = true;
                    break;
                }
            }
            if(!added)
                sorted.Add(arr);
        }

        Console.WriteLine(string.Join('\n',sorted.Select(x => x.ToString().Replace("\r","").Replace("\n","").Replace(" ",""))));
        //var firstIndex = input.TakeWhile(x => x < 2).Count();
        //var secondIndex = input.TakeWhile(x => x < 6).Count() + 1;

        //return firstIndex * secondIndex;
        return (sorted.IndexOf(firstPacket) + 1) * (sorted.IndexOf(secondPacket) + 1);
    }

    private static int GetArrayKey(JsonArray arr)
    {
        foreach (var item in arr)
        {
            return item is not JsonArray ? int.Parse(item.ToString()) : GetArrayKey(item.AsArray());
        }

        return -1;
    }

    private static bool? IsRightOrder(JsonArray a, JsonArray b)
    {
        for (int i = 0; i < a.Count && i < b.Count; i++)
        {
            if (a[i] is not JsonArray && b[i] is not JsonArray)
            {
                var itemA = int.Parse(a[i].ToString());
                var itemB = int.Parse(b[i].ToString());

                if (itemA < itemB)
                    return true;
                if (itemA > itemB)
                    return false;
            }
            else if (a[i] is not JsonArray)
            {
                var arr = new JsonArray { a[i].ToString() };
                var result = IsRightOrder(arr, b[i].AsArray());
                if (result.HasValue)
                    return result.Value;
            }
            else if (b[i] is not JsonArray)
            {
                var arr = new JsonArray { b[i].ToString() };
                var result = IsRightOrder(a[i].AsArray(), arr);
                if (result.HasValue)
                    return result.Value;
            }
            else
            {
                var result = IsRightOrder(a[i].AsArray(), b[i].AsArray());
                if (result.HasValue)
                    return result.Value;
            }
        }
        if(a.Count == b.Count)
            return null;
        return a.Count < b.Count;
    }
}
