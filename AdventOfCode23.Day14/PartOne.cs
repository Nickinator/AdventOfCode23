using System.ComponentModel;

namespace AdventOfCode23.Day14;

static class SolutionDay14Part01
{
    enum ObjectType { Empty = 0, Rounded, Cubical }
    record Position(int Row, int Col, ObjectType Type)
    {
        public ObjectType Type { get; set; } = Type;
    }
    class Platform
    {
        HashSet<Position> _positions;
        int _rowCount;
        int _colCount;

        public Platform(HashSet<Position> positions)
        {
            _positions = positions;
            _rowCount = positions.Select(p => p.Row).Max() + 1;
            _colCount = positions.Select(p => p.Col).Max() + 1;
            TiltNorth();
        }

        private void TiltNorth()
        {
            for (int r = 1; r < _rowCount; r++)
            {
                for (int c = 0; c < _colCount; c++) // skip first row
                {
                    var current = _positions.Where(p => p.Row == r && p.Col == c).First();
                    if (current.Type is ObjectType.Rounded)
                    {
                        MoveNorth(current);
                    }
                }
            }
        }

        private void MoveNorth(Position current)
        {
            var rowIndex = current.Row;
            while (rowIndex > 0)
            {
                var northNeighbor = _positions.Where(p => p.Row == rowIndex - 1 && p.Col == current.Col).First();
                if (northNeighbor.Type is ObjectType.Empty)
                {
                    rowIndex--;
                }
                else
                {
                    break;
                }
            }

            if (rowIndex != current.Row)
            {
                var dest = _positions.Where(p => p.Row == rowIndex && p.Col == current.Col).First();
                dest.Type = ObjectType.Rounded;
                current.Type = ObjectType.Empty; // rock rolled away
            }    
        }

        public int CalculateTotalLoad()
        {
            return _positions
                .Where(p => p.Type is ObjectType.Rounded)
                .Select(p => _rowCount - p.Row)
                .Sum();
        }
    }

    public static void Solution()
    {
        var lines = File.ReadAllLines("input14.txt").ToList();
        var positions = Parse(lines);
        var platform = new Platform(positions);

        var totalLoad = platform.CalculateTotalLoad();
        Console.WriteLine(totalLoad);
    }

    static HashSet<Position> Parse(List<string> lines)
    {
        var positions = new HashSet<Position>();
        for (int r = 0; r < lines.Count; r++)
        {
            for (int c = 0; c < lines[r].Length; c++)
            {
                var type = lines[r][c] switch
                {
                    '.' => ObjectType.Empty,
                    'O' => ObjectType.Rounded,
                    '#' => ObjectType.Cubical,
                    _ => throw new InvalidEnumArgumentException()
                };
                positions.Add(new Position(r, c, type));
            }
        }
        return positions;
    }
}