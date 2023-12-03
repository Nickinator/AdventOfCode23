using System.Text.RegularExpressions;

namespace AdventOfCode23.Day03;

static class SolutionDay03Part02
{
    record Part(int PartNumber, int RowIndex, int StartColIndex, int EndColIndex);
    record Symbol(char SymbolChar, int RowIndex, int ColIndex);

    public static void Solution()
    {
        var lines = File.ReadAllLines("input03.txt").ToList();
        var (parts, symbols) = Parse(lines);

        var gearCandidates = symbols.Where(s => s.SymbolChar is '*');  
        var sumOfGearRatios = 0;
        foreach (var candidate in gearCandidates)
        {
            var adjacentParts = parts.Where(p => IsAdjacent(p, candidate));
            if (adjacentParts.Count() == 2)
            {
                sumOfGearRatios += adjacentParts
                    .Select(p => p.PartNumber)
                    .Aggregate((x, y) => x * y);
            }
        }

        Console.WriteLine(sumOfGearRatios);
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
            new Symbol(
                char.Parse(m.Value),
                index,
                m.Index));
    }

    static bool IsAdjacent(Part part, Symbol symbol)
    {
        return symbol.ColIndex >= part.StartColIndex - 1
            && symbol.ColIndex <= part.EndColIndex + 1
            && symbol.RowIndex >= part.RowIndex - 1
            && symbol.RowIndex <= part.RowIndex + 1;
    }
}