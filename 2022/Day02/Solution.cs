namespace AdventOfCode.Y2022.Day02;

[ProblemName("Rock Paper Scissors")]
class Solution : Solver
{

    public object PartOne(string input)
    {
        var lines = input.Split('\n');
        var h = lines.Select(x =>
        {
            var y = x.Split(' ');
            return new Tuple<string, string>(y[0], y[1]);
        });

        var score = 0;
        foreach (var c in h)
        {
            score += CalcScoreFromCodes(c.Item2, c.Item1);
            //Console.WriteLine(score);
        }

        return score;
    }

    public object PartTwo(string input)
    {
        var lines = input.Split('\n');
        var h = lines.Select(x =>
        {
            var y = x.Split(' ');
            return new Tuple<string, string>(y[0], y[1]);
        });

        var score = 0;
        foreach (var c in h)
        {
            score += CalcScoreWithExpectedResult(c.Item1, c.Item2);
            //Console.WriteLine(score);
        }

        return score;
    }

    private const int WinScore = 6;
    private const int DrawScore = 3;
    private const int LoseScore = 0;
    private const string Rock = "Rock";
    private const string Paper = "Paper";
    private const string Scissors = "Scissors";

    private readonly Dictionary<string, string> codeShapes = new Dictionary<string, string>()
    {
        {"A", Rock}, { "B", Paper }, { "C", Scissors },
        {"X", Rock}, { "Y", Paper }, { "Z", Scissors },
    };
    private readonly Dictionary<string, int> shapeScores = new Dictionary<string, int>()
    {
        {"Rock", 1}, { "Paper", 2 }, { "Scissors", 3 },
    };
    private readonly Dictionary<string, int> winScores = new Dictionary<string, int>()
    {
        {$"{Rock}{Rock}", DrawScore}, { $"{Paper}{Paper}", DrawScore }, {$"{Scissors}{Scissors}", DrawScore },
        {$"{Rock}{Scissors}", WinScore}, { $"{Scissors}{Paper}", WinScore }, { $"{Paper}{Rock}", WinScore },
        {$"{Rock}{Paper}", LoseScore}, { $"{Paper}{Scissors}", LoseScore }, { $"{Scissors}{Rock}", LoseScore },
    };

    public int CalcScoreFromCodes(string myCode, string theirCode)
    {
        // A for Rock, B for Paper, and C for Scissors
        // X for Rock, Y for Paper, and Z for Scissors
        // values 1 for Rock, 2 for Paper, and 3 for Scissors
        // scores 0 if you lost, 3 if the round was a draw, and 6 if you won
        // total score = shapeScore + winScore
        var myShape = codeShapes[myCode];
        var theirShape = codeShapes[theirCode];
        var shapeScore = shapeScores[myShape];
        var winScore = winScores[myShape + theirShape];
        var totalScore = shapeScore + winScore;
        //Console.WriteLine($"{myShape}({myCode}) vs {theirShape}({theirCode}) ({myShape + theirShape}) gives shape:{shapeScore} and win:{winScore} giving {totalScore}");
        return totalScore;

    }
    /* part 2 */
    private const string Win = "Win";
    private const string Lose = "Lose";
    private const string Draw = "Draw";

    private readonly Dictionary<string, string> intendedResult = new Dictionary<string, string>()
    {
        {"X", Lose}, { "Y", Draw }, { "Z", Win },
    };

    public int CalcScoreWithExpectedResult(string theirCode, string expectedResult)
    {
        // A for Rock, B for Paper, and C for Scissors
        // X means you need to lose, Y means you need to end the round in a draw, and Z means you need to win.
        // values 1 for Rock, 2 for Paper, and 3 for Scissors
        // scores 0 if you lost, 3 if the round was a draw, and 6 if you won
        // total score = shapeScore + winScore
        var theirShape = codeShapes[theirCode];
        var result = intendedResult[expectedResult];

        var intendedScore = result == Win ? WinScore : result == Lose ? LoseScore : DrawScore;
        var match = winScores.Single(x => x.Value == intendedScore && x.Key.EndsWith(theirShape));
        var myShape = match.Key.Substring(0, match.Key.Length - theirShape.Length);
        var shapeScore = shapeScores[myShape];
        var winScore = winScores[myShape + theirShape];
        var totalScore = shapeScore + winScore;
        //Console.WriteLine($"Must {result}: {myShape} vs {theirShape}({theirCode}) ({myShape + theirShape}) gives shape:{shapeScore} and win:{winScore} giving {totalScore}");
        return totalScore;

    }
}
