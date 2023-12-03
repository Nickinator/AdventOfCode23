using System.Text.RegularExpressions;

namespace AdventOfCode23.Day03;

static class SolutionDay03Part01
{
    record Part(int PartNumber, int RowIndex, int StartColIndex, int EndColIndex);
    record Symbol(int RowIndex, int ColIndex);

    public static void Solution()
    {
        var lines = File.ReadAllLines("input03.txt").ToList();
        var (parts, symbols) = Parse(lines);

        var sumOfParts = 0;
        foreach(var part in parts)
        {
            var candidates = symbols.Where(s =>
                s.RowIndex <= part.RowIndex + 1
                && s.RowIndex >= part.RowIndex - 1);

            if (candidates.Any(c => IsAdjacent(part, c)))
            {
                sumOfParts += part.PartNumber;
            }
        }

        Console.WriteLine(sumOfParts);
    }

    static (List<Part>, List<Symbol>) Parse(List<string> lines)
    {
        var parts = new List<Part>();
        var symbols = new List<Symbol>();

        for (int i = 0; i < lines.Count; i++)
        {
            parts.AddRange(ParseParts(lines[i], i));
            symbols.AddRange(ParseSymbols(lines[i], i));        
        }

        return (parts, symbols);
    }

    static IEnumerable<Part> ParseParts(string line, int index)
    {
        var pattern = @"\d+";
        var regex = new Regex(pattern);
        var matches = regex.Matches(line);

        return matches.Select(m =>
            new Part(
                int.Parse(m.Value),
                index,
                m.Index,
                m.Index + m.Length - 1));
    }

    static IEnumerable<Symbol> ParseSymbols(string line, int index)
    {
        var pattern = @"[^\d.]";
        var regex = new Regex(pattern);
        var matches = regex.Matches(line);

        return matches.Select(m =>
            new Symbol(index, m.Index));
    }

    static bool IsAdjacent(Part part, Symbol symbol)
    {
        // No need to check for row index as it's already filtered
        return symbol.ColIndex >= part.StartColIndex - 1
            && symbol.ColIndex <= part.EndColIndex + 1;
    }
}