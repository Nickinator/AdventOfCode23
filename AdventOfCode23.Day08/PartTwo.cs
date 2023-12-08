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
        var pathLengths = Traverse(directions, nodes);

        var stepCount = pathLengths
            .Aggregate((x, y) => LCM(x, y));
            
        Console.WriteLine(stepCount);
    }

    static long GCD(long a, long b)
    {
        return a == 0
            ? b 
            : b == 0
                ? a
                : GCD(b, a % b); 
    }

    static long LCM(long a, long b)
    {
        return a * b / GCD(a, b); 
    }

    static IEnumerable<long> Traverse(List<Direction> directions, List<Node> nodes)
    {
        List<string> currentIds = nodes
            .Where(n => n.Id.EndsWith('A'))
            .Select(n => n.Id)
            .ToList();

        var pathLengths = currentIds
            .Select(id => FindPathLength(id, directions, nodes));
        return pathLengths;
    }

    static long FindPathLength(string startId, List<Direction> directions, List<Node> nodes)
    {
        int count = 0;
        int index = 0; // assigned here for debugging
        var currentId = startId;
        while (!currentId.EndsWith('Z'))
        {
            index = count % directions.Count;
            var dir = directions[index];
            var currentNode = nodes
                .Where(n => n.Id == currentId)
                .ElementAt(0);
            currentId = dir == Direction.Left
                ? currentNode.NextLeft
                : currentNode.NextRight;
            count++;
        }

        var debug = $"StartId: {startId} EndId: {currentId} Count: {count} Index: {index}";
        Console.WriteLine(debug);
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