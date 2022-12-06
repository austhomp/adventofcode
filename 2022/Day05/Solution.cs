using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2022.Day05;

[ProblemName("Supply Stacks")]      
class Solution : Solver {

    public object PartOne(string input)
    {
        var problem = ParseProblem(input);
        var initialStacks = CreateStacks(problem.Stacks);
        var finalStacks = MoveStacksIndividually(initialStacks, problem.Moves);

        var result = finalStacks.Stacks.ToList().Select(s => s.Pop()).ToArray();
        return new string(result); 
    }

    public object PartTwo(string input) {
        var problem = ParseProblem(input);
        var initialStacks = CreateStacks(problem.Stacks);
        var finalStacks = MoveStacksTogether(initialStacks, problem.Moves);

        var result = finalStacks.Stacks.ToList().Select(s => s.Pop()).ToArray();
        return new string(result);
    }

    record ProblemStatement(string[] Stacks, string[] Moves);

    record CrateStacks(Stack<char>[] Stacks);

    ProblemStatement ParseProblem(string s)
    {
         var sections = s.Split("\n\n");
         var stackLines = sections[0].Split('\n');
         var moveLines = sections[1].Split('\n');
         return new ProblemStatement(stackLines, moveLines);
    }

    CrateStacks CreateStacks(string[] problemStacks)
    {
        var stacks = new List<Stack<char>>();
        Enumerable.Range(1,9).ToList().ForEach(x => stacks.Add(new Stack<char>()));
        foreach (var line in problemStacks.Reverse().Skip(1))
        {
            var stackIndex = 0;
            foreach (var x in line.Skip(1).Chunk(4).ToList())
            {
                if (x[0] != ' ')
                {
                    stacks[stackIndex].Push(x.First());
                }
                stackIndex++;
            }
        }

        //Console.WriteLine(new String(stacks[0].ToList().ToArray()));
        return new CrateStacks(stacks.ToArray());
    }

    private CrateStacks MoveStacksIndividually(CrateStacks initialStacks, string[] problemMoves)
    {
        Stack<char>[] updatedStacks = initialStacks.Stacks;
        foreach (var line in problemMoves)
        {
            Console.WriteLine(line);
            var (count, src, dest) = ParseMoveLine(line);
            Console.WriteLine($"moving {count} from {src} to {dest}");
            for (int i = 0; i < count; i++)
            {
                var val = updatedStacks[src-1].Pop();
                updatedStacks[dest-1].Push(val);
            }

        }
        return new CrateStacks(updatedStacks);
    }
    private CrateStacks MoveStacksTogether(CrateStacks initialStacks, string[] problemMoves)
    {
        Stack<char>[] updatedStacks = initialStacks.Stacks;
        foreach (var line in problemMoves)
        {
            Console.WriteLine(line);
            var (count, src, dest) = ParseMoveLine(line);
            Console.WriteLine($"moving {count} from {src} to {dest}");
            
            // store on a temporary stack so that when we pop the values off it will restore the original order
            var temp = new Stack<char>();
            for (var i = 0; i < count; i++)
            {
                temp.Push(updatedStacks[src - 1].Pop());
            }

            while (temp.Any())
            {
                updatedStacks[dest - 1].Push(temp.Pop());
            }
            
        }
        return new CrateStacks(updatedStacks);
    }

    private (int, int, int) ParseMoveLine(string line)
    {
        var pieces = line.Split(' ');
        var count = int.Parse(pieces[1]);
        var src = int.Parse(pieces[3]);
        var dest = int.Parse(pieces[5]);
        return (count, src, dest);
    }
}
