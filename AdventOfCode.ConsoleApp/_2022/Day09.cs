using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.ConsoleApp._2022;
internal class Day09
{
    private class Point
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
    }


    public static void Execute()
    {
        var data = DataProvider.GetData(2022, 9);
        Console.WriteLine("Advent of Code 2022 / Day 9");
        Console.WriteLine($"Total spaces visited by tail with rope 2: {GetTotalSpacesVisitedByTail(data,2)}");
        Console.WriteLine($"Total spaces visited by tail with rope 10: {GetTotalSpacesVisitedByTail(data,10)}");
    }
    private static long GetTotalSpacesVisitedByTail(string data, int ropeLength)
    {
        var moves = data.Split("\r\n").Select(x => x.Split(' ')).Select(x => (direction:x.First(),count:x.Last()));

        var rope = Enumerable.Range(0, ropeLength).Select(x => new Point(0, 0)).ToArray();

        var visited = new HashSet<(int,int)>();

        foreach (var move in moves)
        {
            var count = int.Parse(move.count);
            for (var i = 0; i < count; i++)
            {
                MoveHead(rope[0], move.direction);

                for (var j = 1; j < rope.Length; j++) 
                    MoveTail(rope[j], rope[j - 1]);

                visited.Add((rope[^1].X, rope[^1].Y));
            }
        }

        return visited.Count;
    }


    private static void MoveHead(Point head, string direction)
    {
        var move = direction switch
        {
            "D" => new Point(0, -1),
            "U" => new Point(0, 1),
            "L" => new Point(-1, 0),
            "R" => new Point(1, 0)
        };

        head.X += move.X;
        head.Y += move.Y;
    }
    private static void MoveTail(Point tail, Point head)
    {
        if (!IsInRange(tail, head))
        {
            tail.X += Math.Sign(head.X - tail.X);
            tail.Y += Math.Sign(head.Y - tail.Y);
        }
    }

    private static bool IsInRange(Point a, Point b) => Math.Abs(a.X - b.X) <= 1 && +Math.Abs(a.Y - b.Y) <= 1;

    
}
