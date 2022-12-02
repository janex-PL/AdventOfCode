using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.ConsoleApp._2015
{
    public class Day08
    {
        public static void Execute()
        {
            Console.WriteLine(float.Epsilon);
            var lines = DataProvider.GetData(2015, 8).Split("\r\n").ToArray();

            int totalCode = lines.Sum(l => l.Length);
            int totalCharacters = lines.Sum(CharacterCount);
            int totalEncoded = lines.Sum(EncodedStringCount);

            Console.WriteLine(totalCode - totalCharacters);
            Console.WriteLine(totalEncoded - totalCode);
        }
        static int CharacterCount(string arg) => Regex.Match(arg, @"^""(\\x..|\\.|.)*""$").Groups[1].Captures.Count;
        static int EncodedStringCount(string arg) => 2 + arg.Sum(CharsToEncode);
        static int CharsToEncode(char c) => c == '\\' || c == '\"' ? 2 : 1;
    }
}
