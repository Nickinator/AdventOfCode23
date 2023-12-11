namespace AdventOfCode23.Day11;

static class SolutionDay11Part01
{
    record Location(int Row, int Col, bool IsGalaxy)
    {
        public int Row { get; set; } = Row;
        public int Col { get; set; } = Col;
        public bool IsGalaxy { get; init; } = IsGalaxy;
        public int Distance(Location other) => 
            Math.Abs(Row - other.Row) + Math.Abs(Col - other.Col);
    }

    class Universe
    {
        List<Location> _locations;

        public Universe(IEnumerable<Location> locations)
        {
            _locations = locations.ToList();
            ExpandRows();
            ExpandCols();
        }

        private void ExpandRows()
        {
            var rowCount = _locations.Select(loc => loc.Row).Max();
            var offset = 0;
            for (int r = 0; r < rowCount; r++)
            {
                var row = _locations.Where(loc => loc.Row == r + offset);
                if (row.All(loc => loc.IsGalaxy is false))
                {
                    foreach (var location in _locations.Where(loc => loc.Row > r + offset))
                    {
                        location.Row++;
                    }
                    offset++;
                }
            }
        }

        private void ExpandCols()
        {
            var colCount = _locations.Select(loc => loc.Col).Max();
            var offset = 0;
            for (int c = 0; c < colCount; c++)
            {
                var col = _locations.Where(loc => loc.Col == c + offset);
                if (col.All(loc => loc.IsGalaxy is false))
                {
                    foreach (var location in _locations.Where(loc => loc.Col > c + offset))
                    {
                        location.Col++;
                    }
                    offset++;
                }
            }
        }

        public IEnumerable<int> ShortestPaths()
        {
            var galaxies = _locations.Where(loc => loc.IsGalaxy is true);
            for (int i = 0; i < galaxies.Count() - 1; i++)
            {
                for (int j = i + 1; j < galaxies.Count(); j++)
                {
                    yield return galaxies.ElementAt(i).Distance(galaxies.ElementAt(j));
                }
            }
        }
    }
    public static void Solution()
    {
        var lines = File.ReadAllLines("input11.txt").ToList();
        var locations = Parse(lines);
        var universe = new Universe(locations);

        var sumOfShortestPaths = universe.ShortestPaths().Sum();
        Console.WriteLine(sumOfShortestPaths);
    }

    static IEnumerable<Location> Parse(List<string> lines)
    {
        for (int r = 0; r < lines.Count; r++)
        {
            for (int c = 0; c < lines[r].Length; c++)
            {
                var current = lines[r][c];
                yield return current is '.'
                    ? new Location(r, c, false)
                    : new Location(r, c, true);
            }
        }
    }
}