using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2022.Day04;

[ProblemName("Camp Cleanup")]      
class Solution : Solver {

    // input looks like 7-50,8-33
    public object PartOne(string input)
    {
        //input = "2-4,6-8\n2-3,4-5\n5-7,7-9\n2-8,3-7\n6-6,4-6\n2-6,4-8";
        var lines = input.Split('\n');
        var splits = lines.Select(s =>
        {
            var halves = s.Split(',').Select(x => x.Split('-').ToArray()).ToArray();
            return  halves;
        }).ToList();
        
        var pairs = splits.Select(x =>
        {
            var first = x[0];
            var second = x[1];
            return new { First = new { Low = int.Parse(first[0]), High = int.Parse(first[1]) }, Second= new { Low = int.Parse(second[0]), High = int.Parse(second[1]) }};
        }).ToList();

        //pairs.ForEach(x => Console.WriteLine($"{x.First.Low}-{x.First.High} and {x.Second.Low}-{x.Second.High} = {AreSubsets(x.First.Low, x.First.High, x.Second.Low, x.Second.High)}"));

        var val = pairs.Count(x => AreSubsets(x.First.Low, x.First.High, x.Second.Low, x.Second.High));
        //Console.WriteLine($"Total pairs: {pairs.Count}");
        var checks = pairs.Zip(lines).ToList().Select(t => new
        {
            Orig = t.Second,
            Computed = $"{t.First.First.Low}-{t.First.First.High},{t.First.Second.Low}-{t.First.Second.High}",
            IsSubset = AreSubsets(t.First.First.Low, t.First.First.High, t.First.Second.Low, t.First.Second.High)
        }).ToList();

        //checks.Where(arg => arg.IsSubset).ToList().ForEach(o => Console.WriteLine($"{o.Orig}"));
        var allGood = checks.All(arg => arg.Orig == arg.Computed);
        //Console.WriteLine($"Check subset pairs: {checks.Where(arg => arg.IsSubset).Count()}"); 
        //Console.WriteLine($"All match: {allGood}");
        return val;
    }

    private bool AreSubsets(int leftLow, int leftHigh, int rightLow, int rightHigh)
    {
       var left = Enumerable.Range(leftLow, leftHigh - leftLow + 1).ToList();
        var right = Enumerable.Range(rightLow, rightHigh - rightLow + 1).ToList();
        return !left.Except(right).Any() || !right.Except(left).Any();
    }

    public object PartTwo(string input) {
        var lines = input.Split('\n');
        var splits = lines.Select(s =>
        {
            var halves = s.Split(',').Select(x => x.Split('-').ToArray()).ToArray();
            return halves;
        }).ToList();
        var pairs = splits.Select(x =>
        {
            var first = x[0];
            var second = x[1];
            return new { First = new { Low = int.Parse(first[0]), High = int.Parse(first[1]) }, Second = new { Low = int.Parse(second[0]), High = int.Parse(second[1]) } };
        }).ToList();
        var val = pairs.Count(x => AreOverlapped(x.First.Low, x.First.High, x.Second.Low, x.Second.High));
        return val;
    }

    private bool AreOverlapped(int leftLow, int leftHigh, int rightLow, int rightHigh)
    {
        var left = Enumerable.Range(leftLow, leftHigh - leftLow + 1).ToList();
        var right = Enumerable.Range(rightLow, rightHigh - rightLow + 1).ToList();
        return left.Intersect(right).Any();
    }

}
