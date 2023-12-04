using System.Text.RegularExpressions;

namespace AdventOfCode23.Day04;

static class SolutionDay04Part01
{
    class ScratchCard
    {
        List<int> _winningNumbers = new();
        List<int> _actualNumbers = new();

        public ScratchCard(List<int> winningNumbers, List<int> actualNumbers)
        {
            _winningNumbers = winningNumbers;
            _actualNumbers = actualNumbers;
        }

        public int CalculateValue()
        {
            var numOfWinning = _winningNumbers
                .Intersect(_actualNumbers)
                .Count();
                
            return numOfWinning > 0 ? (int)Math.Pow(2, numOfWinning - 1) : 0;
        }
    }
    public static void Solution()
    {
        var lines = File.ReadAllLines("input04.txt").ToList();
        var cards = Parse(lines);

        var sumOfCardValues = cards
            .Select(c => c.CalculateValue())
            .Sum();
    
        Console.WriteLine(sumOfCardValues);
    }

    private static List<ScratchCard> Parse(List<string> lines)
    {
        List<ScratchCard> cards = new();
        foreach (var line in lines)
        {
            cards.Add(ParseCard(line));
        }
        return cards;
    }

    private static ScratchCard ParseCard(string cardString)
    {
        var parts = cardString.Split("|");
        var pattern = @"\d+";
        var regex = new Regex(pattern);

        var winningNumbers = regex
            .Matches(parts[0])
            .Skip(1) // skip card number
            .Select(m => int.Parse(m.Value))
            .ToList();

        var actualNumbers = regex
            .Matches(parts[1])
            .Select(m => int.Parse(m.Value))
            .ToList();

        return new ScratchCard(winningNumbers, actualNumbers);
    }
}