using System.Text.RegularExpressions;

namespace AdventOfCode23.Day02;

static class SolutionDay02Part02
{
    record Set(int Red, int Green, int Blue)
    {
        public int Power => Red * Green * Blue;
    }
    class Game
    {
        readonly List<Set> _sets;
        public int Id { get; init; }

        public Game(int id, List<Set> sets)
        {
            Id = id;
            _sets = sets;
        }

        public Set GetMinimumBagContent()
        {
            var minRed = _sets.Select(set => set.Red).Max();
            var minGreen = _sets.Select(set => set.Green).Max();
            var minBlue = _sets.Select(set => set.Blue).Max();

            return new Set(minRed, minGreen, minBlue);
        }
    }
    public static void Solution()
    {
        var lines = ReadToListOfLines("input02.txt");
        List<Game> games = Parse(lines);

        var sumOfPowers = games
            .Select(game => game
                .GetMinimumBagContent().Power)
            .Sum();

        Console.WriteLine(sumOfPowers);
    }

    private static List<string> ReadToListOfLines(string filename)
    {
        return File.ReadAllLines(filename).ToList();
    }

    private static List<Game> Parse(List<string> lines)
    {
        var games = new List<Game>();
        foreach (var line in lines)
        {
            games.Add(ParseLine(line));
        }

        return games;
    }

    private static Game ParseLine(string gameString)
    {
        var pattern = @"^Game (\d+):\s(.*)$";
        var regex = new Regex(pattern);
        var match = regex.Match(gameString);
        var gameID = int.Parse(match.Groups[1].Value);
        var remainder = match.Groups[2].Value.Replace(" ", "");
        var setStrings = remainder.Split(';');
        
        var sets = new List<Set>();
        foreach (var setString in setStrings)
        {
            sets.Add(ParseSet(setString));
        }

        return new Game(gameID, sets);
    }

    private static Set ParseSet(string setString)
    {
        int red = 0, green = 0, blue = 0;
        var cubes = setString.Split(',');
        var pattern = @"^(\d+)(.*)$";
        var regex = new Regex(pattern);

        foreach (var cube in cubes)
        {
            var match = regex.Match(cube);
            var count = int.Parse(match.Groups[1].Value);
            var color = match.Groups[2].Value;

            if (color is "red") red = count;
            else if (color is "green") green = count;
            else blue = count;
        }

        return new Set(red, green, blue);
    }
}