using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.ConsoleApp._2021
{
    public class Day10
    {
        public static void Execute()
        {
            var data = DataProvider.GetData(2021, 10).Split("\r\n");
            Console.WriteLine(GetSyntaxErrorScore(data));
            Console.WriteLine(GetSyntaxIncompleteWinner(data));
        }

        private static long GetSyntaxErrorScore(string[] data)
        {
            var score = 0L;
            foreach (var line in data)
            {
                var openingBrackets = new List<char>();
                for (int i = 0; i < line.Length; i++)
                {
                    if("<({[".Contains(line[i]))
                        openingBrackets.Add(line[i]);
                    else
                    {
                        var expectedBracket = line[i] switch
                        {
                            ')' => '(',
                            '>' => '<',
                            '}' => '{',
                            ']' => '[',
                        };
                        if (!openingBrackets.Any() || openingBrackets.Last() != expectedBracket)
                        {
                            score += line[i] switch
                            {
                                ')' => 3,
                                '>' => 25137,
                                '}' => 1197,
                                ']' => 57
                            };
                            break;
                        }
                        openingBrackets.RemoveAt(openingBrackets.Count-1);
                    }
                }
            }

            return score;

        }

        private static long GetSyntaxIncompleteWinner(string[] data)
        {
            var scores = new List<long>();
            foreach (var line in data)
            {
                var openingBrackets = new List<char>();
                for (int i = 0; i < line.Length; i++)
                {
                    if("<({[".Contains(line[i]))
                        openingBrackets.Add(line[i]);
                    else
                    {
                        var expectedBracket = line[i] switch
                        {
                            ')' => '(',
                            '>' => '<',
                            '}' => '{',
                            ']' => '[',
                        };
                        if (!openingBrackets.Any() || openingBrackets.Last() != expectedBracket)
                        {
                            openingBrackets.Clear();
                            break;
                        }
                        openingBrackets.RemoveAt(openingBrackets.Count-1);
                    }
                }

                if (openingBrackets.Any())
                {
                    openingBrackets.Reverse();
                    var score = 0L;
                    foreach (var openingBracket in openingBrackets)
                    {
                        score *= 5;
                        score += openingBracket switch
                        {
                           '(' => 1,
                          '<' => 4,
                          '{' => 3,
                           '[' => 2,
                        };
                    }
                    scores.Add(score);
                }
            }

            return scores.OrderBy(x => x).ToList()[scores.Count / 2];
        }
    }
}
