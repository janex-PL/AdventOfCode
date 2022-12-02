using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.ConsoleApp._2021
{
    public class Day04
    {
        public static void Execute()
        {
            var data = DataProvider.GetData(2021, 4);
            Console.WriteLine(GetBingoResult(data.Split("\r\n")));
            Console.WriteLine(GetBingoResultLast(data.Split("\r\n")));
        }

        private static long GetBingoResult(string[] data)
        {
            var numbersToDraw = data.First().Split(',').Select(int.Parse).ToList();
            var boardsData = data.Skip(1).Where(x => !string.IsNullOrEmpty(x)).ToList();
            var boards = new List<List<(int,bool)>>();

            for (var i = 0; i < boardsData.Count / 5; i++)
            {
                boards.Add(string.Join(' ',boardsData.Skip(5*i).Take(5)).Split(' ').Where(x => !string.IsNullOrEmpty(x)).Select(x => (int.Parse(x),false)).ToList());
            }

            foreach (var number in numbersToDraw)
            {
                foreach (var board in boards)
                {
                    var index = board.IndexOf((number, false));
                    if (index != -1)
                        board[index] = (number, true);
                    if (IsBingo(board))
                        return board.Where(x => !x.Item2).Select(x => x.Item1).Sum() * number;
                }
            }

            return -1;
        }
        private static long GetBingoResultLast(string[] data)
        {
            var numbersToDraw = data.First().Split(',').Select(int.Parse).ToList();
            var boardsData = data.Skip(1).Where(x => !string.IsNullOrEmpty(x)).ToList();
            var boards = new List<List<(int,bool)>>();

            for (var i = 0; i < boardsData.Count / 5; i++)
            {
                boards.Add(string.Join(' ',boardsData.Skip(5*i).Take(5)).Split(' ').Where(x => !string.IsNullOrEmpty(x)).Select(x => (int.Parse(x),false)).ToList());
            }

            foreach (var number in numbersToDraw)
            {
                foreach (var board in boards)
                {
                    var index = board.IndexOf((number, false));
                    if (index != -1)
                        board[index] = (number, true);
                }
                if(boards.Count > 1)
                    boards = boards.Where(x => !IsBingo(x)).ToList();
                else if(IsBingo(boards.First()))
                    return boards.First().Where(x => !x.Item2).Select(x => x.Item1).Sum() * number;
            }
            
            return -1;
        }
        private static bool IsBingo(List<(int,bool)> board)
        {
            for (int i = 0; i < 5; i++)
            {
                var row = Enumerable.Range(0, 5).Select(x => (Row: i, Col: x)).ToList();
                var column = Enumerable.Range(0, 5).Select(x => (Row: x,Col: i)).ToList();

                if (row.All(cords => board[cords.Row * 5 + cords.Col].Item2) ||
                    column.All(cords => board[cords.Row * 5 + cords.Col].Item2))
                    return true;
            }

            return false;
        }
    }
}
