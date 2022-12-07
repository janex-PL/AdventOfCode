using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode.ConsoleApp._2022;
public class Day05
{
    public static void Execute()
    {
        var data = DataProvider.GetData(2022, 5);
        Console.WriteLine("Advent of Code 2022 / Day 5");
        Console.WriteLine($"Top packages value: {GetTopPackages(data,false)}");
        Console.WriteLine($"Top packages value with preserved order: {GetTopPackages(data,true)}");
    }

    private class Stack
    {
        public int Id { get; }
        public Stack<char> Packages { get; } = new();

        public Stack(int id)
        {
            Id = id;
        }
    }

    private class Instruction
    {
        public int Size { get; }
        public int From { get; }
        public int To { get; }

        public Instruction(string line)
        {
            var inputs = Regex.Matches(line, @"\d+").Select(x => int.Parse(x.Value)).ToList();

            Size = inputs[0];
            From = inputs[1];
            To = inputs[2];
        }
    }

    private static string GetTopPackages(string data, bool preservedOrder)
    {
        var stacks = ExtractStacks(data);

        var instructions = ExtractInstructions(data);

        foreach (var ins in instructions)
        {
            if (preservedOrder)
                MovePackagesWithPreservedOrder(stacks, ins);
            else
                MovePackages(stacks, ins);
        }

        return string.Concat(stacks.Select(s => s.Packages.Peek()));
    }

    private static void MovePackages(List<Stack> source, Instruction instruction)
    {
        for (var i = 0; i < instruction.Size; i++)
        {
            source[instruction.To-1].Packages.Push(source[instruction.From-1].Packages.Pop());
        }
    }

    private static void MovePackagesWithPreservedOrder(List<Stack> source, Instruction instruction)
    {
        var stack = new Stack<char>();
        while(stack.Count < instruction.Size)
            stack.Push(source[instruction.From-1].Packages.Pop());
        while(stack.Count > 0)
            source[instruction.To-1].Packages.Push(stack.Pop());
    }

    private static List<Instruction> ExtractInstructions(string data)
    {
        var lines = data.Split("\r\n");
        var boundary = lines.Select((x, i) => (x, i)).First(x => x.x == "").i;

        return lines.Skip(boundary+1).Select(x => new Instruction(x)).ToList();
    }

    private static List<Stack> ExtractStacks(string data)
    {
        var lines = data.Split("\r\n");

        var boundary = lines.Select((x, i) => (x, i)).First(x => x.x == "").i;

        var stacks = lines[boundary - 1].Split(' ').Where(x => !string.IsNullOrEmpty(x))
            .Select(x => new Stack(int.Parse(x))).ToList();

        for (var i = boundary - 2; i >= 0; i--)
        {
            for (var j = 1; j < lines[i].Length; j += 4)
            {
                if(lines[i][j] != ' ')
                    stacks[j/4].Packages.Push(lines[i][j]);
            }
        }

        return stacks;
    }
}
