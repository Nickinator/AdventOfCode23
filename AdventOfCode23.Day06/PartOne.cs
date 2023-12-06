using System.Text.RegularExpressions;

namespace AdventOfCode23.Day06;

static class SolutionDay06Part01
{
    record Race(int Duration, int RecordDistance);
    public static void Solution()
    {
        var lines = File.ReadAllLines("input06.txt").ToList();
        var races = Parse(lines);

        var procuctOfWaysToWin = races
            .Select(r => GetNumberOfWaysToWin(r))
            .Aggregate((x, y) => x * y);

        Console.WriteLine(procuctOfWaysToWin);
    }

    static List<Race> Parse(List<string> lines)
    {
        var races = new List<Race>();
        var durationString = lines[0];
        var distanceString = lines[1];
        var pattern = @"\d+";
        var regex = new Regex(pattern);
        var durations = regex.Matches(durationString).Select(m => int.Parse(m.Value));
        var distances = regex.Matches(distanceString).Select(m => int.Parse(m.Value));
        races.AddRange(
            durations.Zip(distances, (dur, dist) => new Race(dur, dist)));

        return races;
    }

    static int GetNumberOfWaysToWin(Race race)
    {
        var waysToWin = 0;
        for (int i = 0; i <= race.Duration; i++)
        {
            var chargeTime = i;
            var moveTime = race.Duration - i;
            if (chargeTime * moveTime > race.RecordDistance)
            {
                waysToWin++;
            }
        }

        return waysToWin;
    }
}