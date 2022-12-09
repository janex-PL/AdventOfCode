using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.ConsoleApp._2022;
public class Day08
{
    public static void Execute()
    {
        var data = DataProvider.GetData(2022, 8);
        Console.WriteLine("Advent of Code 2022 / Day 8");
        Console.WriteLine($"Total visible trees {GetTotalVisibleTrees(data)}");
        Console.WriteLine($"Highest scenic score {GetHighestScenicScore(data)}");
    }

    private static List<List<int>> ParseData(string data) =>
        data.Split("\r\n").Select(x => x.Select(y => int.Parse(y.ToString())).ToList()).ToList();

    private static int GetHighestScenicScore(string data)
    {
        List<List<int>> trees = data.Split("\r\n").Select(x => x.Select(y => int.Parse(y.ToString())).ToList()).ToList();

        var rows = trees.Count;
        var columns = trees.First().Count;

        var result = 0;

        for (var i = 0; i < rows; i++)
        {
            for (var j = 0; j < columns; j++)
            {
                var height = trees[i][j];

                if (i == 0 || j == 0 || i == rows - 1 || j == columns - 1)
                    continue;

                var index = 1;
                var (up, down, left, right) = (true, true, true, true);
                var (upScore, downScore, leftScore, rightScore) = (0, 0, 0, 0);

                while (up || down || left || right)
                {
                    if (up)
                    {
                        if (i - index >= 0 && trees[i - index][j] <= height)
                        {
                            upScore++;
                            if (trees[i - index][j] == height)
                                up = false;
                        }
                        else
                            up = false;
                    }

                    if (down)
                    {
                        if (i + index < rows && trees[i + index][j] <= height)
                        {
                            downScore++;
                            if (trees[i + index][j] == height)
                                down = false;
                        }
                        else
                            down = false;
                    }

                    if (left)
                    {
                        if (j - index >= 0 && trees[i][j - index] <= height)
                        {
                            leftScore++;
                            if (trees[i][j - index] == height)
                                left = false;
                        }
                        else
                            left = false;
                    }

                    if (right)
                    {
                        if (j + index < columns && trees[i][j + index] <= height)
                        {
                            rightScore++;
                            if (trees[i][j + index] == height)
                                right = false;
                        }
                        else
                            right = false;
                    }

                    index++;
                }

                result = Math.Max(result, upScore * downScore * leftScore * rightScore);
            }
        }

        return result;
    }

    private static int GetTotalVisibleTrees(string data)
    {
        var trees = data.Split("\r\n").Select(x => x.Select(y => int.Parse(y.ToString())).ToList()).ToList();

        var rows = trees.Count;
        var columns = trees.First().Count;

        var result = 0;

        for (var i = 0; i < rows; i++)
        {
            for (var j = 0; j < columns; j++)
            {
                var height = trees[i][j];

                if (i == 0 || j == 0 || i == rows - 1 || j == columns - 1)
                    result++;
                else if (trees[i].Take(j).All(x => x < height))
                    result++;
                else if (trees[i].Skip(j + 1).All(x => x < height))
                    result++;
                else if(trees.Where((_,ind)=> ind < i).All(x => x[j] < height))
                    result++;
                else if(trees.Where((_,ind)=> ind > i).All(x => x[j] < height))
                    result++;
            }
        }

        return result;
    }


}
