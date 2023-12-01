using System.Text.RegularExpressions;

namespace AdventOfCode23.Day01;

class SolutionDay01Part02
{
    static readonly Dictionary<string, string> _wordToDigitMap = new() 
        {
            {"one", "1"},
            {"two", "2"},
            {"three", "3"},
            {"four", "4"},
            {"five", "5"},
            {"six", "6"},
            {"seven", "7"},
            {"eight", "8"},
            {"nine", "9"},
        };

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
        var keys = _wordToDigitMap.Keys;
        var pattern = @"\d|" + string.Join('|', keys); // match any digit or digit word
        var overlapPattern = @"(?=" + pattern + ")"; // account for overlaps using look-ahead

        var regex = new Regex(pattern);
        var overlapRegex = new Regex(overlapPattern);
        var matchIndices = overlapRegex.Matches(line).Select(match => match.Index);

        var firstDigit = GetDigitValue(
            regex.Match(line, matchIndices.First()).Value);
        
        var secondDigit = GetDigitValue(
            regex.Match(line, matchIndices.Last()).Value);   
        
        return int.Parse(firstDigit + secondDigit);
    }

    private static string GetDigitValue(string match)
    {
        return _wordToDigitMap.TryGetValue(match, out var digitValue)
            ? digitValue
            : match;
    }

    private static List<string> ReadToListOfLines(string filename)
    {
        return File.ReadAllLines(filename).ToList();
    }
}