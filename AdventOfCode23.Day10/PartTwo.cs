using System.ComponentModel;

namespace AdventOfCode23.Day10;

static class SolutionDay10Part02
{
    record Position(int Row, int Col);
    enum ConnectorType { None = 0, NorthSouth, EastWest, NorthEast, NorthWest, SouthWest, SouthEast }
    record Tile(Position Pos, ConnectorType Type)
    {
        public ConnectorType Type { get; set; } = Type;
        public (Position, Position) GetNeighbors()
            => Type switch
            {
                ConnectorType.NorthSouth => (new Position(Pos.Row - 1, Pos.Col), new Position(Pos.Row + 1, Pos.Col)),
                ConnectorType.EastWest => (new Position(Pos.Row, Pos.Col + 1), new Position(Pos.Row, Pos.Col - 1)),
                ConnectorType.NorthEast => (new Position(Pos.Row - 1, Pos.Col), new Position(Pos.Row, Pos.Col + 1)),
                ConnectorType.NorthWest => (new Position(Pos.Row - 1, Pos.Col), new Position(Pos.Row, Pos.Col - 1)),
                ConnectorType.SouthWest => (new Position(Pos.Row + 1, Pos.Col), new Position(Pos.Row, Pos.Col - 1)),
                ConnectorType.SouthEast => (new Position(Pos.Row + 1, Pos.Col), new Position(Pos.Row, Pos.Col + 1)),
                _ => throw new InvalidEnumArgumentException()
            };
    }
    class Map
    {
        HashSet<Tile> _tiles = new();
        HashSet<Position> _visited = new();
        Tile _start = null!;
        const char START_TOKEN = '-'; // hardcoded for simplicity
        int _loopLength = 0;
        public int MaxDist { get => _loopLength / 2; }
        Dictionary<char, ConnectorType> _tileTable = new()
        {
            {'.', ConnectorType.None},
            {'|', ConnectorType.NorthSouth},
            {'-', ConnectorType.EastWest},
            {'L', ConnectorType.NorthEast},
            {'J', ConnectorType.NorthWest},
            {'7', ConnectorType.SouthWest},
            {'F', ConnectorType.SouthEast}
        };

        public Map(List<string> lines)
        {
            Parse(lines);
            Traverse(_start);
            RemoveScrapTiles();
        }

        private void RemoveScrapTiles()
        {
            foreach (var tile in _tiles)
            {
                if (_visited.Contains(tile.Pos))
                {
                    continue;
                }

                tile.Type = ConnectorType.None;
            }
        }

        public int CalculateEnclosedTileCount()
        {
            int count = 0;
            var verticals = new List<ConnectorType>()
                {ConnectorType.NorthSouth, ConnectorType.NorthWest, ConnectorType.NorthEast};

            var nonLoopTiles = _tiles.Where(t => !_visited.Contains(t.Pos));
            foreach (var tile in nonLoopTiles)
            {
                var leftTiles = _tiles.Where(t =>
                    t.Pos.Row == tile.Pos.Row && t.Pos.Col < tile.Pos.Col);

                var verticalCount = leftTiles
                    .Where(t => verticals.Contains(t.Type))
                    .Count();
                    
                if (verticalCount % 2 != 0)
                {
                    count++;
                }
            }

            return count;
        }

        private void Traverse(Tile currentTile)
        {
            while (!_visited.Contains(currentTile.Pos))
            {
                _visited.Add(currentTile.Pos);
                var (neighbor1, neighbor2) = currentTile.GetNeighbors();
                var pos = _visited.Contains(neighbor1) ? neighbor2 : neighbor1;
                currentTile = GetTile(pos);
                _loopLength++;
            }
        }

        private void Parse(List<string> lines)
        {
            for (int r = 0; r < lines.Count; r++)
            {
                for (int c = 0; c < lines[r].Length; c++)
                {
                    var current = lines[r][c];
                    var pos = new Position(r, c);
                    if (current == 'S')
                    {
                        _tiles.Add(new Tile(pos, _tileTable[START_TOKEN])); // hardcoded
                        _start = GetTile(pos);
                        continue;
                    }
                    _tiles.Add(new Tile(pos, _tileTable[current]));
                }
            }
        }

        private Tile GetTile(Position pos)
        {
            return _tiles
                .Where(t =>
                    t.Pos.Row == pos.Row
                    && t.Pos.Col == pos.Col)
                .First();
        }
    }

    public static void Solution()
    {
        var lines = File.ReadAllLines("input10.txt").ToList();
        var map = new Map(lines);

        var enclosedTileCount = map.CalculateEnclosedTileCount();
        Console.WriteLine(enclosedTileCount);
    }
}