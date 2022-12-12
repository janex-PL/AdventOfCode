using System;

namespace AdventOfCode.ConsoleApp._2022
{
    public delegate void DayDelegate();
    public class DaySelector
    {
        public static void LaunchDay(int day)
        {
            DayDelegate entryPoint = day switch
            {
                1 => Day01.Execute,
                2 => Day02.Execute,
                3 => Day03.Execute,
                4 => Day04.Execute,
                5 => Day05.Execute,
                6 => Day06.Execute,
                7 => Day07.Execute,
                8 => Day08.Execute,
                9 => Day09.Execute, 
                10 => Day10.Execute,
                11 => Day11.Execute,
                12 => Day12.Execute,
                _ => throw new ArgumentOutOfRangeException(nameof(day))
            };
            entryPoint();
        }
    }
}
