using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2022.Day03;

[ProblemName("Rucksack Reorganization")]      
class Solution : Solver {

    public object PartOne(string input)
    {
        var chunks = input.Split('\n').Select(s =>
        {
            var chars = s.ToCharArray();
            var chunks = chars.Chunk(chars.Length / 2).ToList();
            return new { Left = chunks.First(), Right = chunks.Last() };
        }).ToList();
        //chunks.ForEach(s => Console.WriteLine($"{new string(s.Left)} {new string(s.Right)}"));

        var lettersInCommon = chunks.Select(x => new { InCommon = x.Left.Intersect(x.Right)}).ToList();
        lettersInCommon.ForEach(x => Console.WriteLine(new string(x.InCommon.ToArray())));

        var sum = lettersInCommon.Sum(arg => ItemToPriority(arg.InCommon.First()));
        Console.WriteLine(ItemToPriority('A'));
        Console.WriteLine((int)'A');
        //var areAllSameLength = chunks.All(arg => arg.Left.Length == arg.Right.Length);
        //Console.WriteLine($"same length:{areAllSameLength}");
        return sum;
    }

    public object PartTwo(string input) {
        var groups = input.Split('\n').Chunk(3).Select(x => x.First().ToArray().Intersect(x.Skip(1).First().ToArray()).Intersect(x.Last().ToArray()));
        //groups.ToList().ForEach(x => Console.WriteLine(new string(x.ToArray())));
        var sum = groups.Sum(arg => ItemToPriority(arg.First()));
        return sum;
    }

    private int ItemToPriority(char c)
    {
        return char.IsUpper(c) ? (int)c - (int)'A' + 27 : (int)c - (int)'a' + 1;
    }

}
