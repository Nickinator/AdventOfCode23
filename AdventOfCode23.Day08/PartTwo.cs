using System.Text.RegularExpressions;

namespace AdventOfCode23.Day08;

static class SolutionDay08Part02
{
        record Node(string Id, string NextLeft, string NextRight);
    enum Direction {Left = 0, Right}

    public static void Solution()
    {
        var lines = File.ReadAllLines("input08.txt").ToList();
        var (directions, nodes) = Parse(lines);
        var stepCount = Traverse(directions, nodes);
        Console.WriteLine(stepCount);
    }

    static int Traverse(List<Direction> directions, List<Node> nodes)
    {
        List<string> currentIds = nodes
            .Where(n => n.Id.EndsWith('A'))
            .Select(n => n.Id)
            .ToList();
        int count = 0;

        while (!currentIds.All(i => i.EndsWith('Z')))
        {
            var dir = directions[count % directions.Count];
            var currentNodes = nodes.Where(n => currentIds.Contains(n.Id));
            currentIds = currentNodes
                .Select(n => dir == Direction.Left
                    ? n.NextLeft
                    : n.NextRight)
                .ToList();

            count++;
        }

        return count;
    }

    private static (List<Direction>, List<Node>) Parse(List<string> lines)
    {
        var directions = ParseDirections(lines[0]);
        var nodes = new List<Node>();
        lines.RemoveRange(0, 2);
        foreach (var line in lines)
        {
            nodes.Add(ParseNode(line));
        }

        return (directions, nodes);
    }

    private static List<Direction> ParseDirections(string directionString)
    {
        var directions = new List<Direction>();
        foreach (var dir in directionString)
        {
            var direction = dir == 'L' ? Direction.Left : Direction.Right;
            directions.Add(direction);
        }
        return directions;
    }

    private static Node ParseNode(string nodeString)
    {
        var pattern = @"^([A-Z0-9]{3}).*?([A-Z0-9]{3}).*?([A-Z0-9]{3})";
        var regex = new Regex(pattern);
        var match = regex.Match(nodeString);
        var (id, left, right) = (
            match.Groups[1].Value,
            match.Groups[2].Value,
            match.Groups[3].Value);

        return new Node(id, left, right);
    }
}