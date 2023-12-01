using System.Text.RegularExpressions;

namespace AdventOfCode23.Day01;

static class SolutionDay01Part01
{
    public static void Solution()
    {
        var lines = ReadToListOfLines("input01.txt");

        int sumOfCalibrationValues = lines
            .Select(line => GetCalibrationValue(line))
            .Sum();

        Console.WriteLine(sumOfCalibrationValues);
    }

    private static int GetCalibrationValue(string line)
    {
        string pattern = @"\d"; // match all digits
        var regex = new Regex(pattern);
        var matches = regex.Matches(line);
        var firstDigit = matches.First().Value;
        var secondDigit = matches.Last().Value;
        return int.Parse(firstDigit + secondDigit);
    }

    private static List<string> ReadToListOfLines(string filename)
    {
        return File.ReadAllLines(filename).ToList();
    }
}