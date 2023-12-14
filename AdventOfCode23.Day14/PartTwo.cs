using System.ComponentModel;
using System.Text;

namespace AdventOfCode23.Day14;

static class SolutionDay14Part02
{
    enum ObjectType { Empty = 0, Rounded, Cubical }
    record Position(int Row, int Col, ObjectType Type)
    {
        public int Row { get; set; } = Row;
        public int Col { get; set; } = Col;
        public ObjectType Type { get; set; } = Type;
        public override string ToString()
        {
            return $"{Row},{Col},{Type};";
        }
    }
    class Platform
    {
        HashSet<Position> _positions;
        Dictionary<string, int> _configurations = new();
        int _rowCount;
        int _colCount;
        const int _CYCLE_COUNT = 1_000_000_000;

        public Platform(HashSet<Position> positions)
        {
            _positions = positions;
            _rowCount = positions.Select(p => p.Row).Max() + 1;
            _colCount = positions.Select(p => p.Col).Max() + 1;
            SimulateCycles();
        }

        private void SimulateCycles()
        {
            for (int i = 0; i < _CYCLE_COUNT; i++)
            {
                Console.WriteLine($"Cycle: {i}");
                var config = GetConfigurationAsString();
                if (_configurations.TryGetValue(config, out int index))
                {
                    var cycleLength = i - index;
                    Console.WriteLine($"Cycle Detected after {i} iterations. Cycle length: {cycleLength}");
                    var openLoops = _CYCLE_COUNT - i;
                    var remainder = openLoops % cycleLength;
                    i = _CYCLE_COUNT - remainder; // skip almost to the end
                }

                _configurations.TryAdd(config, i);
                Cycle();
            }
        }

        private string GetConfigurationAsString()
        {
            var builder = new StringBuilder();
            foreach (var pos in _positions)
            {
                builder.Append(pos.ToString());
            }

            return builder.ToString();
        }

        private void Cycle() // Tilt North, West, South, East
        {
            for (int i = 0; i < 4; i++)
            {
                Tilt();
                RotateClockwise();
            }
        }

        private void Tilt()
        {
            for (int r = 1; r < _rowCount; r++)
            {
                for (int c = 0; c < _colCount; c++) // skip first row
                {
                    var current = _positions.Where(p => p.Row == r && p.Col == c).First();
                    if (current.Type is ObjectType.Rounded)
                    {
                        Move(current);
                    }
                }
            }
        }

        private void Move(Position current)
        {
            var rowIndex = current.Row;
            while (rowIndex > 0)
            {
                var nextNeighbor = _positions.Where(p => p.Row == rowIndex - 1 && p.Col == current.Col).First();
                if (nextNeighbor.Type is ObjectType.Empty)
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

        private void RotateClockwise()
        {
            foreach (var pos in _positions)
            {
                var currentRow = pos.Row;
                pos.Row = pos.Col;
                pos.Col = _rowCount - currentRow - 1;
            }

            (_colCount, _rowCount) = (_rowCount, _colCount);
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