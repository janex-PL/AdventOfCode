#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.ConsoleApp._2022;
public class Day07
{
    public static void Execute()
    {
        var data = DataProvider.GetData(2022, 7);
        Console.WriteLine("Advent of Code 2022 / Day 7");
        Console.WriteLine($"Total filesystem size: {GetTotalSize(data)}");
        Console.WriteLine($"Size of directory to be deleted: {GetMinimumDirectorySize(data)}");
    }

    private static long GetTotalSize(string data)
    {
        var root = Parse(data);

        return root.Find(item => item is Directory && item.Size <= 100_000).Sum(item => item.Size);
    }

    private static long GetMinimumDirectorySize(string data)
    {
        var root = Parse(data);

        var freeSpace = 70_000_000 - root.Size;

        var spaceNeeded = 30_000_000 - freeSpace;

        return root.Find(item => item is Directory && item.Size >= spaceNeeded ).OrderBy(item => item.Size).First().Size;
    }


    private static Directory Parse(string data)
    {
        var lines = data.Split("\r\n");

        var currentDir = new Directory(null!,"/");

        var i = 1;
        while (i < lines.Length)
        {
            if (Regex.IsMatch(lines[i], @"\$ cd [a-zA-Z]+"))
            {
                currentDir = (Directory)currentDir.Children[lines[i].Split(' ').Last()];
                i++;
            }
            else if (Regex.IsMatch(lines[i], @"\$ cd \.\."))
            {
                currentDir = currentDir.Parent!;
                i++;
            }
            else if (Regex.IsMatch(lines[i], @"\$ ls"))
            {
                i++;

                var content = lines.Skip(i).TakeWhile(l => !l.StartsWith('$')).ToArray();

                currentDir.AddItems(content);

                i += content.Length;
            }
        }

        while (currentDir.Parent != null)
        {
            currentDir = currentDir.Parent;
        }

        return currentDir;
    }
}
public interface IItem
{
    public Directory Parent { get; }
    public string Name { get; }
    public long Size { get; }

    public string GetPath();
}

public class File : IItem
{
    public Directory Parent { get; }
    public string Name { get; }
    public long Size { get; }

    public string GetPath() =>  Parent.GetPath() + Name + "/";

    public File(Directory parent, string name, long size)
    {
        Parent = parent;
        Name = name;
        Size = size;
    }
}

public class Directory : IItem
{
    public Directory? Parent { get; }
    public string Name { get; }

    public Dictionary<string,IItem> Children {get;} = new();

    public long Size => Children.Sum(c => c.Value.Size);

    public Directory(Directory parent, string name)
    {
        Parent = parent;
        Name = name;
    }

    public string GetPath() =>  Parent is null ? Name : Parent.GetPath() + Name + "/";

    public IEnumerable<IItem> Find(Func<IItem,bool> query)
    {
        var result = new List<IItem>();
        foreach (var item in Children.Values)
        {
            if(query.Invoke(item))
                result.Add(item);
                
            if(item is Directory directory)
                result.AddRange(directory.Find(query));
        }
        return result;
    }

    public void AddItems(IEnumerable<string> items)
    {
        foreach (var item in items)
        {
            var itemContent = item.Split(' ');

            if (!Children.TryGetValue(itemContent.First(), out _))
            {
                IItem child = itemContent.First().Equals("dir")
                    ? new Directory(this, itemContent.Last())
                    : new File(this, itemContent.Last(), long.Parse(itemContent.First()));

                Children.Add(child.Name,child);
            }
        }
    }
}
