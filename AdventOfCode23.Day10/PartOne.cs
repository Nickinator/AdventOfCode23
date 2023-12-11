using System.ComponentModel;
using System.Xml;

namespace AdventOfCode23.Day10;

static class SolutionDay10Part01
{
    record Position(int Row, int Col);
    enum ConnectorType { None = 0, NorthSouth, EastWest, NorthEast, NorthWest, SouthWest, SouthEast }
    record Tile(Position Pos, ConnectorType Type)
    {
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
        Tile _start;
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
        }

        public int Traverse()
        {
            _visited.Add(_start.Pos);
            int distance = 0;
            var (backwards, forwards) = _start.GetNeighbors();
            
            while (true)
            {
                distance++;
                
                forwards = Move(forwards);
                if (_visited.Contains(forwards))
                    break;

                backwards = Move(backwards);
                if (_visited.Contains(backwards))
                {
                    break;
                }
            }

            return distance;
        }

        private Position Move(Position pos)
        {
            _visited.Add(pos);
            var (neighbor1, neighbor2) = GetTile(pos).GetNeighbors();
            pos = _visited.Contains(neighbor1) ? neighbor2 : neighbor1;
            return pos;
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
                        _tiles.Add(new Tile(pos, _tileTable['-']));
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

        var maxDistance = map.Traverse();
        Console.WriteLine(maxDistance);
    }
}