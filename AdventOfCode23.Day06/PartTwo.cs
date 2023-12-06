using System.Text.RegularExpressions;

namespace AdventOfCode23.Day06;

static class SolutionDay06Part02
{
    record Race(long Duration, long RecordDistance);
    public static void Solution()
    {
        var lines = File.ReadAllLines("input06.txt").ToList();
        var race = Parse(lines);
        var waysToWin = GetNumberOfWaysToWin(race);
        
        Console.WriteLine(waysToWin);
    }

    static Race Parse(List<string> lines)
    {
        var durationString = lines[0].Replace(" ", "");
        var distanceString = lines[1].Replace(" ", "");
        var pattern = @"\d+";
        var regex = new Regex(pattern);
        var duration = long.Parse(regex.Match(durationString).Value);
        var distance = long.Parse(regex.Match(distanceString).Value);

        return new Race(duration, distance);
    }

    static int GetNumberOfWaysToWin(Race race)
    {
        var waysToWin = 0;
        for (long i = 0; i <= race.Duration; i++)
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