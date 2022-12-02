using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.ConsoleApp._2015
{
    public class Day09
    {
        public static void Execute()
        {
            var data = DataProvider.GetData(2015, 9).Split("\r\n");
            Console.WriteLine(GetShortestRoute(data));
        }

        private static int GetShortestRoute(string[] data)
        {
            var cities = new List<City>();
            foreach (var line in data)
            {
                var citiesLine = Regex.Matches(line, "[A-Z][A-Za-z]+").Select(x => x.Value).ToList();
                var value = int.Parse(Regex.Match(line, @"\d+").Value);
                var firstCity = cities.FirstOrDefault(x => x.Name == citiesLine.First());
                if (firstCity == null)
                {
                    firstCity = new City() { Name = citiesLine.First() };
                    cities.Add(firstCity);
                }
                var secondCity = cities.FirstOrDefault(x => x.Name == citiesLine.Last());
                if (secondCity == null)
                {
                    secondCity = new City() { Name = citiesLine.Last() };
                    cities.Add(secondCity);
                }
                firstCity.Destinations.Add(secondCity.Name,value);
                secondCity.Destinations.Add(firstCity.Name,value);
            }

            var shortest = int.MaxValue;
            foreach (var city in cities)
            {
                var result = GetRouteDistance(cities, city, new List<City>() { city }, 0);
                if (result < shortest)
                    shortest = result;
            }

            return shortest;
        }

        private static int GetRouteDistance(List<City> cities, City city, List<City> visited, int i)
        {
            if (!cities.Except(visited).Any())
                return i;
            var shortest = int.MaxValue;
            foreach (var destination in city.Destinations.Where(x=> visited.All(y => y.Name != x.Key)))
            {
                i += destination.Value;
                var newCity = cities.First(x => x.Name == destination.Key);
                visited.Add(newCity);
                var result = GetRouteDistance(cities, newCity, visited, i);
                if (result < shortest)
                    shortest = result;
            }

            return shortest;
        }
    }

    public class City
    {
        public string Name { get; set; }
        public Dictionary<string, int> Destinations { get; set; } = new Dictionary<string, int>();

    }
}
