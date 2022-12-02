using System;
using System.Globalization;
using AdventOfCode.ConsoleApp.Data;

namespace AdventOfCode.ConsoleApp
{
    public static class DataProvider
    {
        public static string GetData(int year, int day)
        {
            var resource = year switch
            {
                2015 => AdventOfCode2015Data.ResourceManager,
                2021 => AdventOfCode2021Data.ResourceManager,
                2022 => AdventOfCode2022Data.ResourceManager,
                _ => throw new ArgumentOutOfRangeException(nameof(year))
            };

            return resource.GetResourceSet(CultureInfo.CurrentCulture, true, true)?.GetString($"{day}") ??
                   throw new ArgumentOutOfRangeException(nameof(day));
        }
    }
}