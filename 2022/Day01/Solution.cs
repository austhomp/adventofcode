using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2022.Day01;

[ProblemName("Calorie Counting")]      
class Solution : Solver {

    public object PartOne(string input) {
        var lines = input.Split('\n');
        var outputs = new List<int>();
        var currTotal = 0;
        foreach(var line in lines) {
            if (string.IsNullOrEmpty(line)) {
                outputs.Add(currTotal);
                currTotal = 0;
                continue;
            }
            currTotal += int.Parse(line);
        }

        // Console.WriteLine(outputs.Count);
        var max = outputs.Max();

        // at first I misread the problem and though we wanted to know *which* elf and not their total...
        // var idx = outputs.IndexOf(max);
        return max;
    }

    public object PartTwo(string input) {
        var lines = input.Split('\n');
        var outputs = new List<int>();
        var currTotal = 0;
        foreach(var line in lines) {
            if (string.IsNullOrEmpty(line)) {
                outputs.Add(currTotal);
                currTotal = 0;
                continue;
            }
            currTotal += int.Parse(line);
        }
        var total = outputs.Order().Reverse().Take(3).Sum();

        return total;
    }
}
