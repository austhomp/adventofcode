using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2022.Day06;

[ProblemName("Tuning Trouble")]      
class Solution : Solver {


    private readonly List<KeyValuePair<string, int>> _partOneTestCases = new List<KeyValuePair<string, int>>()
    {
        new("mjqjpqmgbljsphdztnvjfqwrcgsmlb", 7), 
        new("bvwbjplbgvbhsrlpgdmjqwftvncz", 5),
        new("nppdvjthqldpwncqszvftbrmjlhg", 6), 
        new("nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg", 10),
        new("zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw", 11)
    };

    private readonly List<KeyValuePair<string, int>> _partTwoTestCases = new List<KeyValuePair<string, int>>()
    {
        new("mjqjpqmgbljsphdztnvjfqwrcgsmlb", 19),
        new("bvwbjplbgvbhsrlpgdmjqwftvncz", 23),
        new("nppdvjthqldpwncqszvftbrmjlhg", 23),
        new("nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg", 29),
        new("zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw", 26)
    };


    /* Notes
     * start-of-packet marker is four characters that are all different.
     * number of characters from the beginning of the buffer to the end of the first such four-character marker
     */
    public object PartOne(string input)
    {
        foreach (var testCase in _partOneTestCases)
        {
            var startPositions = GetMarkerLocations(testCase.Key, 4);
            var first = startPositions.First() + 1;
            Console.WriteLine($"Part 1 test case expected {testCase.Value} actual was {first} which is {testCase.Value == first}");
        }

        return GetMarkerLocations(input, 4).First() + 1;
    }

    /*
     * Notes - now it's 14 character length
     */
    public object PartTwo(string input) {
        foreach (var testCase in _partTwoTestCases)
        {
            var startPositions = GetMarkerLocations(testCase.Key, 14);
            var first = startPositions.First() + 1;
            Console.WriteLine($"Part 2 est case expected {testCase.Value} actual was {first} which is {testCase.Value == first}");
        }

        return GetMarkerLocations(input, 14).First() + 1;
    }

    private int[] GetMarkerLocations(string msg, int markerLength)
    {
        var array = msg.ToCharArray();
        var q = new Queue<char>();
        q.Enqueue('?'); // filler
        array.Take(markerLength - 1).ToList().ForEach(x => q.Enqueue(x));

        var locs = new List<int>();
        for (int i = markerLength - 1; i < array.Length; i++)
        {
            q.Dequeue();
            q.Enqueue(array[i]);
            if (q.GroupBy(c => c).Count() == markerLength)
            {
                locs.Add(i);
            }
        }

        return locs.ToArray();
    }

    private int[] OriginalGetMarkerLocations(string msg)
    {
        var array = msg.ToCharArray();
        var q = new Queue<char>();
        q.Enqueue('?'); // filler
        q.Enqueue(array[0]);
        q.Enqueue(array[1]);
        q.Enqueue(array[2]);
        var locs = new List<int>();
        for (int i = 3; i < array.Length; i++)
        {
            q.Dequeue();
            q.Enqueue(array[i]);
            if (q.GroupBy(c => c).Count() == 4)
            {
                locs.Add(i);
            }
        }

        return locs.ToArray();
    }

}
