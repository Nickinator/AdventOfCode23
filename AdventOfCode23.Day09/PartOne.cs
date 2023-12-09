using System.Runtime.Versioning;
using System.Text.RegularExpressions;

namespace AdventOfCode23.Day09;

static class SolutionDay09Part01
{
    public static void Solution()
    {
        var lines = File.ReadAllLines("input09.txt").ToList();
        var report = Parse(lines);

        var sumOfNextValues = report
            .Select(d => ExtrapolateSequence(d))
            .Sum();

        Console.WriteLine(sumOfNextValues);
    }

    static int ExtrapolateSequence(List<int> dataset)
    {
        if (dataset.All(n => n is 0))
        {
            return 0;
        }

        var differences = dataset
            .Zip(
                dataset.Skip(1),
                (current, next) => next - current)
            .ToList();

        dataset.Add(dataset.Last() + ExtrapolateSequence(differences));
        return dataset.Last();
    }

    static List<List<int>> Parse(List<string> lines)
    {
        var report = new List<List<int>>();
        foreach (var line in lines)
        {
            report.Add(ParseDataset(line));
        }
        return report;
    }

    static List<int> ParseDataset(string line)
    {
        var pattern = @"-?\d+";
        var regex = new Regex(pattern);
        var matches = regex.Matches(line);
        return matches
            .Select(m => int.Parse(m.Value))
            .ToList();
    }
}