using System.Text.RegularExpressions;

namespace AdventOfCode23.Day08;

static class SolutionDay08Part01
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
        string startId = "AAA";
        string endId = "ZZZ";
        string currentId = startId;
        int count = 0;

        while (currentId != endId)
        {
            var dir = directions[count % directions.Count];
            var currentNode = nodes
                .Where(n => n.Id == currentId)
                .ElementAt(0);
            currentId = dir == Direction.Left
                ? currentNode.NextLeft
                : currentNode.NextRight;
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
        var pattern = @"^([A-Z]{3}).*?([A-Z]{3}).*?([A-Z]{3})";
        var regex = new Regex(pattern);
        var match = regex.Match(nodeString);
        var (id, left, right) = (
            match.Groups[1].Value,
            match.Groups[2].Value,
            match.Groups[3].Value);

        return new Node(id, left, right);
    }
}