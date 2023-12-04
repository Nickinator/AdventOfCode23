using System.Text.RegularExpressions;

namespace AdventOfCode23.Day04;

static class SolutionDay04Part02
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

        public int GetMatchCount()
        {
            return _winningNumbers
                .Intersect(_actualNumbers)
                .Count();
        }
    }
    public static void Solution()
    {
        var lines = File.ReadAllLines("input04.txt").ToList();
        var cards = Parse(lines);

        var matchCounts = cards
            .Select((c, i) => (value: c.GetMatchCount(), index: i + 1))
            .ToDictionary(pair => pair.index, pair => pair.value);

        var cardCounts = matchCounts.ToDictionary(item => item.Key, item => 1);
        var totalCardCount = CountCards(matchCounts, cardCounts);
    
        Console.WriteLine(totalCardCount);
    }

    private static int CountCards(Dictionary<int, int> matchCounts, Dictionary<int, int> cardCounts)
    {
        for (int i = 1; i <= matchCounts.Count; i++)
        {
            var multiplier = cardCounts[i];
            var matchCount = matchCounts[i];
            Enumerable
                .Range(start: i + 1, count: matchCount)
                .ToList()
                .ForEach(ix => cardCounts[ix] += multiplier);
        }

        return cardCounts
            .Select(item => item.Value)
            .Sum();
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