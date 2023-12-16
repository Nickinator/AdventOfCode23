using System.ComponentModel;
using System.Data;

namespace AdventOfCode23.Day16;

static class SolutionDay16Part01
{
    enum Direction { Up = 0, Down, Left, Right }
    record Tile(int Row, int Col, char Type, bool IsEnergized = false)
    {
        public bool IsEnergized { get; set; } = IsEnergized;
    }
    class Contraption
    {
        readonly HashSet<Tile> _tiles;
        readonly HashSet<(int row, int col, Direction dir)> _visited = new();
        readonly int _rowCount;
        readonly int _colCount;

        public Contraption(HashSet<Tile> tiles)
        {
            _tiles = tiles;
            _rowCount = _tiles.Select(t => t.Row).Max() + 1;
            _colCount = _tiles.Select(t => t.Col).Max() + 1;
            Move(GetTile(0, 0), Direction.Right);
        }

        private void Move(Tile current, Direction dir)
        {
            var state = (current.Row, current.Col, dir); // prevent infinite loops
            if (_visited.Contains(state))
            {
                return;
            }
            _visited.Add(state);

            current.IsEnergized = true;
            var nextDirs = GetNextDirection(current.Type, dir);

            foreach (var nextDir in nextDirs)
            {
                var (nextRow, nextCol) = ApplyDirection(current, nextDir);
                if (IsOutOfBounds(nextRow, nextCol))
                {
                    continue;
                }
                Move(GetTile(nextRow, nextCol), nextDir);
            }
        }

        private List<Direction> GetNextDirection(char type, Direction directionTo)
        {
            return (type, directionTo) switch
            {
                ('.', _) => new() { directionTo },
                ('|', Direction.Right) or ('|', Direction.Left) => new() { Direction.Up, Direction.Down },
                ('|', Direction.Up) or ('|', Direction.Down) => new() { directionTo },
                ('-', Direction.Right) or ('-', Direction.Left) => new() { directionTo },
                ('-', Direction.Up) or ('-', Direction.Down) => new() { Direction.Left, Direction.Right },
                ('/', Direction.Left) => new() { Direction.Down },
                ('/', Direction.Right) => new() { Direction.Up },
                ('/', Direction.Up) => new() { Direction.Right },
                ('/', Direction.Down) => new() { Direction.Left },
                ('\\', Direction.Left) => new() { Direction.Up },
                ('\\', Direction.Right) => new() { Direction.Down },
                ('\\', Direction.Up) => new() { Direction.Left },
                ('\\', Direction.Down) => new() { Direction.Right },
                _ => throw new ArgumentException()
            };
        }

        private static (int row, int col) ApplyDirection(Tile tile, Direction direction)
        {
            return direction switch
            {
              Direction.Up => (tile.Row - 1, tile.Col),
              Direction.Down => (tile.Row + 1, tile.Col),
              Direction.Left => (tile.Row, tile.Col - 1),
              Direction.Right => (tile.Row, tile.Col + 1),
              _ => throw new InvalidEnumArgumentException()  
            };
        }

        private Tile GetTile(int row, int col)
        {
            if (IsOutOfBounds(row, col)) // not actually needed here but hey
            {
                throw new ArgumentOutOfRangeException();
            }

            return _tiles
                .Where(t => t.Row == row && t.Col == col)
                .First();
        }

        private bool IsOutOfBounds(int row, int col)
        {
            return row >= _rowCount
                || row < 0
                || col >= _colCount
                || col < 0;
        }

        public int GetEnergizedCount()
        {
            return _tiles.Where(t => t.IsEnergized).Count();
        }
    }
    public static void Solution()
    {
        var lines = File.ReadAllLines("input16.txt");
        var tiles = Parse(lines);
        var contraption = new Contraption(tiles);

        var energizedCount = contraption.GetEnergizedCount();
        Console.WriteLine(energizedCount);
    }

    private static HashSet<Tile> Parse(string[] lines)
    {
        var tiles = new HashSet<Tile>();
        for (int r = 0; r < lines.Length; r++)
        {
            for (int c = 0; c < lines[r].Length; c++)
            {
                char type = lines[r][c];
                tiles.Add(new Tile(r, c, type));
            }
        }
        return tiles;
    }
}