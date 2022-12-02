using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.ConsoleApp._2020
{
    public static class Day04
    {
        public static void Execute()
        {
            var data = DataProvider.GetData(2020, 4);
            Console.WriteLine(GetValidPassportCount(data));
            Console.WriteLine(GetValidatedPasswords(data));
        }

        private static int GetValidPassportCount(string data)
        {
            var passports = data.Split("\r\n\r\n").Select(x => x.Replace("\r\n", " ").Split(' ').ToList()).ToList();
            return passports.Count;
        }

        private static int GetValidatedPasswords(string data)
        {
            var passports = data.Split("\r\n\r\n").Select(x => x.Replace("\r\n", " ").Split(' ').ToList()).ToList();
            var ctr = 0;
            foreach (var passport in passports)
            {
                if (passport.Count != 8 && (passport.Count != 7 || passport.Any(x => x.Contains("cid:")))) continue;
                var isValid = true;
                foreach (var inputField in passport.Select(field => field.Split(':').ToList()))
                {
                    switch (inputField.First())
                    {
                        case "byr":
                            if (!int.TryParse(inputField.Last(), out var byr) || byr < 1920 || byr > 2002)
                                isValid = false;
                            break;

                        case "iyr":
                            if (!int.TryParse(inputField.Last(), out var iyr) || iyr < 2010 || iyr > 2020)
                                isValid = false;
                            break;

                        case "eyr":
                            if (!int.TryParse(inputField.Last(), out var eyr) || eyr < 2020 || eyr > 2030)
                                isValid = false;
                            break;

                        case "hcl":
                            if (!Regex.IsMatch(inputField.Last(), "#[0-9a-f]{6}"))
                                isValid = false;
                            break;

                        case "ecl":
                            var list = new List<string>() { "amb", "blu", "brn", "gry", "grn", "hzl", "oth" };
                            if (!list.Contains(inputField.Last()))
                                isValid = false;
                            break;

                        case "pid":
                            if (!Regex.IsMatch(inputField.Last(), @"^\d{9}$"))
                                isValid = false;
                            break;

                        case "hgt":
                            if (Regex.IsMatch(inputField.Last(), @"\d+cm"))
                            {
                                var number = int.Parse(Regex.Match(inputField.Last(), @"\d+").Value);
                                if (number < 150 || number > 193)
                                    isValid = false;
                            }
                            else if (Regex.IsMatch(inputField.Last(), @"\d+in"))
                            {
                                var number = int.Parse(Regex.Match(inputField.Last(), @"\d+").Value);
                                if (number < 59 || number > 76)
                                    isValid = false;
                            }
                            else
                            {
                                isValid = false;
                            }

                            break;
                    }

                    if (!isValid)
                        break;
                }

                if (isValid)
                    ctr++;
            }

            return ctr;
        }
    }
}