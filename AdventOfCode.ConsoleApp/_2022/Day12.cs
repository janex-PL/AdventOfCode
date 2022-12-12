using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace AdventOfCode.ConsoleApp._2022;
internal class Day12
{


    public class Point
    {
        public char Value { get; set; }
        public int Heat { get; set; } = -1;
        public List<Point> Exits = new();

        public Point(char value)
        {
            Value = value;
        }
    }

    private static readonly List<(int i, int j)> AllowedMoves = new()
    {
        (0,1),(1,0),(-1,0),(0,-1)
    };

    public static void Execute()
    {
        var data = DataProvider.GetData(2022, 12);
        Console.WriteLine($"Minimum distance from start: {GetMinimumDistance(data, new []{'S'})}");
        Console.WriteLine($"Minimum distance from a: {GetMinimumDistance(data, new []{'S','a'})}");
    }

    private static int GetMinimumDistance(string data, char[] startingPoints)
    {
        var graph = GenerateGraph(data, startingPoints);
        var endPos = FindPoint(graph, '{');
        var ctr = 0;

        while (graph[endPos.i][endPos.j].Heat == -1)
        {
            NextStep(graph, ctr);
            ctr++;
        }

        return graph[endPos.i][endPos.j].Heat;
    }

    private static void NextStep(List<List<Point>> graph, int ctr)
    {
        var points = graph.SelectMany(x => x).Where(x => x.Heat == ctr).ToList();
        foreach (var point in points)
        {
            foreach (var pointExit in point.Exits)
            {
                if (pointExit.Heat == -1)
                    pointExit.Heat = ctr + 1;
            }
        }
    }


    private static List<List<Point>> GenerateGraph(string data, char[] startingPoints)
    {
        var result = data.Replace('E','{').Split("\r\n").Select(x => x.Select(y => new Point(y)).ToList()).ToList();

        for (int i = 0; i < result.Count; i++)
        {
            for (int j = 0; j < result[i].Count; j++)
            {
                var parent = result[i][j];
                if (startingPoints.Contains(parent.Value))
                {
                    parent.Heat = 0;
                    parent.Value = 'a';
                }

                foreach (var move in AllowedMoves)
                {

                    var nextPos = (i: i + move.i, j: j + move.j);

                    if (nextPos.i >= 0 && nextPos.j >= 0 && nextPos.i < result.Count && nextPos.j < result[i].Count )
                    {
                        var child = result[nextPos.i][nextPos.j];

                        if (child.Value - parent.Value <= 1) 
                            result[i][j].Exits.Add(result[nextPos.i][nextPos.j]);
                    }
                }
            }
        }

        return result;
    }

    private static (int i, int j) FindPoint(List<List<Point>> graph, char value)
    {
        for (int i = 0; i < graph.Count; i++)
        {
            for (int j = 0; j < graph[i].Count; j++)
            {
                if (graph[i][j].Value == value)
                    return (i, j);
            }
        }

        throw new Exception();
    }


    private static void DisplayMap(List<List<Point>> graph, int currentCtr)
    {
        for (int i = 0; i < graph.Count; i++)
        {
            for (int j = 0; j < graph[i].Count; j++)
            {
                var point = graph[i][j];
                if(point.Heat == currentCtr)
                    Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(point.Heat == -1 ? point.Value : point.Heat.ToString().Last());
                Console.ForegroundColor = ConsoleColor.White;
            }

            Console.WriteLine();
        }
    }
}
