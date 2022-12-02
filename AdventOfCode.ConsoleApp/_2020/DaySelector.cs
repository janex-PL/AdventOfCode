using System;

namespace AdventOfCode.ConsoleApp._2020
{
    public delegate void DayDelegate();
    public class DaySelector
    {
        public static void LaunchDay(int day)
        {
            DayDelegate entryPoint = day switch
            {
                2 => Day02.Execute,
                3 => Day03.Execute,
                4 => Day04.Execute,
                _ => throw new ArgumentOutOfRangeException(nameof(day))
            };
            entryPoint();
        }
    }
}
