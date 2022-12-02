using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.ConsoleApp._2021
{
    public class Day09
    {
        public static void Execute()
        {
            var data = DataProvider.GetData(2021, 9).Split("\r\n");
            Console.WriteLine(GetRiskLevelSum(data));
            Console.WriteLine(GetBasinsMultiplier(data));
        }

        private static int GetRiskLevelSum(string[] data)
        {
            var sum = 0;
            for (var j = 0; j < data.Length; j++)
            {
                var line = data[j];
                for (int i = 0; i < line.Length; i++)
                {
                    if ((i - 1 < 0 || line[i] < line[i - 1]) &&
                        (i + 1 >= line.Length || line[i] < line[i + 1]) &&
                        (j-1 < 0 || line[i] < data[j-1][i]) &&
                        (j+1 >= data.Length || line[i] < data[j+1][i]))
                        sum += 1 + (line[i] - '0');
                }
            }

            return sum;
        }

        private static int GetBasinsMultiplier(string[] data)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var lowPoints = GetBasinsLowPoints(data);
            var basinsSizes = new List<int>();
            foreach ((int Y, int X) lowPoint in lowPoints)
            {
                var result = GetBasinsPoints(data, lowPoint, new List<(int Y, int X)>(){lowPoint});
                basinsSizes.Add(result.Count);
            }

            var multiply = 1;
            basinsSizes = basinsSizes.OrderByDescending(x => x).Take(3).ToList();
            foreach (var basinsSize in basinsSizes)
            {
                multiply *= basinsSize;
            }
            watch.Stop();
            return multiply;
        }

        private static List<(int Y, int X)> GetBasinsPoints(string[] data, (int Y, int X) currentPoint, List<(int Y, int X)> visitedPoints)
        {
            if (!visitedPoints.Contains((currentPoint.Y, currentPoint.X - 1)) && currentPoint.X - 1 >= 0 &&
                (data[currentPoint.Y][currentPoint.X - 1] != '9' && data[currentPoint.Y][currentPoint.X] <
                    data[currentPoint.Y][currentPoint.X - 1]))
            {
                visitedPoints.Add((currentPoint.Y, currentPoint.X - 1));
                visitedPoints = GetBasinsPoints(data, (currentPoint.Y, currentPoint.X - 1), visitedPoints);
            }
            if (!visitedPoints.Contains((currentPoint.Y, currentPoint.X + 1)) && currentPoint.X + 1  < data.First().Length &&
                (data[currentPoint.Y][currentPoint.X + 1] != '9' && data[currentPoint.Y][currentPoint.X] <
                    data[currentPoint.Y][currentPoint.X + 1]))
            {
                visitedPoints.Add((currentPoint.Y, currentPoint.X + 1));
                visitedPoints = GetBasinsPoints(data, (currentPoint.Y, currentPoint.X + 1), visitedPoints);
            }
            if (!visitedPoints.Contains((currentPoint.Y-1, currentPoint.X)) && currentPoint.Y - 1  >= 0 &&
                (data[currentPoint.Y -1][currentPoint.X] != '9' && data[currentPoint.Y][currentPoint.X] <
                    data[currentPoint.Y -1][currentPoint.X]))
            {
                visitedPoints.Add((currentPoint.Y-1, currentPoint.X));
                visitedPoints = GetBasinsPoints(data, (currentPoint.Y-1, currentPoint.X), visitedPoints);
            }
            if (!visitedPoints.Contains((currentPoint.Y+1, currentPoint.X)) && currentPoint.Y + 1  < data.Length &&
                (data[currentPoint.Y + 1][currentPoint.X] != '9' && data[currentPoint.Y][currentPoint.X] <
                    data[currentPoint.Y + 1][currentPoint.X]))
            {
                visitedPoints.Add((currentPoint.Y+1, currentPoint.X));
                visitedPoints = GetBasinsPoints(data, (currentPoint.Y+1, currentPoint.X), visitedPoints);
            }

            return visitedPoints;

        }

        private static List<(int Y, int X)> GetBasinsLowPoints(string[] data)
        {
            var result = new List<(int Y, int X)>();
            for (var j = 0; j < data.Length; j++)
            {
                var line = data[j];
                for (int i = 0; i < line.Length; i++)
                {
                    if ((i - 1 < 0 || line[i] < line[i - 1]) &&
                        (i + 1 >= line.Length || line[i] < line[i + 1]) &&
                        (j - 1 < 0 || line[i] < data[j - 1][i]) &&
                        (j + 1 >= data.Length || line[i] < data[j + 1][i]))
                        result.Add((j, i));
                }
            }

            return result;
        }
    }
}
