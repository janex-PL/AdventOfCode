using System;

namespace AdventOfCode.ConsoleApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            _2022.Day04.Execute();
            while (true)
            {
                Console.Write("Input year: ");
                var year = int.Parse(Console.ReadLine() ?? string.Empty);
                Console.Write("Input day: ");
                var day = int.Parse(Console.ReadLine() ?? string.Empty);
                switch (year)
                {
                    case 2015:
                        _2015.DaySelector.LaunchDay(day);
                        break;
                    case 2020:
                        _2020.DaySelector.LaunchDay(day);
                        break;
                    case 2021:
                        _2021.DaySelector.LaunchDay(day);
                        break;
                    case 2022:
                        _2022.DaySelector.LaunchDay(day);
                        break;
                }
            }
        }
    }
}