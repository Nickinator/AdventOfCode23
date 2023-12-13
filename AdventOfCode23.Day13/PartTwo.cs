namespace AdventOfCode23.Day13;

static class SolutionDay13Part02
{
    enum ObjectType { Ash = 0, Rock }
    record Position(int Row, int Col, ObjectType Type)
    {
        public ObjectType Type { get; set; } = Type;
    }
    class Formation
    {
        enum ReflectionType { Vertical, Horizontal }
        record ReflectionLine(ReflectionType Type, int Index)
        {
            public ReflectionType Type { get; set; } = Type;
            public int Index { get; set; } = Index;
        }
        HashSet<Position> _positions;
        int _rowCount;
        int _colCount;
        ReflectionLine _reflectionLine;

        public Formation(HashSet<Position> positions)
        {
            _positions = positions;
            _rowCount = _positions.Select(p => p.Row).Max();
            _colCount = _positions.Select(p => p.Col).Max();
            _reflectionLine = FindReflectionLine();
            AccountForSmudges();
        }

        private void AccountForSmudges()
        {
            foreach (var pos in _positions)
            {
                pos.Type = ToggleType(pos.Type);
                var candidateReflection = FindReflectionLine();
                if (IsNewValidReflectionLine(candidateReflection))
                {
                    _reflectionLine = candidateReflection;
                    return;
                }
                pos.Type = ToggleType(pos.Type); // restore original
            }
        }

        private ObjectType ToggleType(ObjectType type)
        {
            return type is ObjectType.Ash ? ObjectType.Rock : ObjectType.Ash;
        }

        private ReflectionLine FindReflectionLine()
        {
            ReflectionType type;
            int index;

            var vertical = CheckVertically();
            if (vertical > 0)
            {
                index = vertical;
                type = ReflectionType.Vertical;
            }
            else
            {
                index = CheckHorizontally();
                type = ReflectionType.Horizontal;                
            }

            return new ReflectionLine(type, index);
        }

        private bool IsNewValidReflectionLine(ReflectionLine line)
        {
            if (line.Index is 0)
            {
                return false;
            }

            if (_reflectionLine is null)
            {
                return true;
            }

            return line != _reflectionLine;
        }

        public int GetFormationSummary()
        {
            return _reflectionLine.Type is ReflectionType.Vertical
                ? _reflectionLine.Index
                : _reflectionLine.Index * 100;
        }

        private int CheckVertically()
        {
            for (int i = 0; i < _colCount; i++)
            {
                var thisCol = _positions.Where(p => p.Col == i);
                var nextCol = _positions.Where(p => p.Col == i + 1);

                if (IsPatternEqual(thisCol, nextCol)
                    && IsVerticalReflection(i, i + 1)
                    && IsNewValidReflectionLine(new ReflectionLine(ReflectionType.Vertical, i + 1)))
                {
                    return i + 1;
                }
            }

            return -1;
        }

        private int CheckHorizontally()
        {
            for (int i = 0; i < _rowCount; i++)
            {
                var thisRow = _positions.Where(p => p.Row == i);
                var nextRow = _positions.Where(p => p.Row == i + 1);

                if (IsPatternEqual(thisRow, nextRow)
                    && IsHorizontalReflection(i, i + 1)
                    && IsNewValidReflectionLine(new ReflectionLine(ReflectionType.Horizontal, i + 1)))
                {
                    return i + 1;
                }
            }

            return 0;
        }

        private bool IsVerticalReflection(int left, int right)
        {
            if (left < 0 || right > _colCount)
            {
                return true;
            }

            var col1 = _positions.Where(p => p.Col == left);
            var col2 = _positions.Where(p => p.Col == right);
            if (IsPatternEqual(col1, col2) is false)
            {
                return false;
            }

            return IsVerticalReflection(left - 1, right + 1);
        }

        private bool IsHorizontalReflection(int above, int below)
        {
            if (above < 0 || below > _rowCount)
            {
                return true;
            }

            var row1 = _positions.Where(p => p.Row == above);
            var row2 = _positions.Where(p => p.Row == below);
            if (IsPatternEqual(row1, row2) is false)
            {
                return false;
            }

            return IsHorizontalReflection(above - 1, below + 1);
        }

        private bool IsPatternEqual(IEnumerable<Position> first, IEnumerable<Position> second)
        {
            return first
                .Zip(second, (a, b) => a.Type == b.Type)
                .All(t => t);
        }
    }
    public static void Solution()
    {
        var lines = File.ReadAllLines("input13.txt").ToList();
        var formations = Parse(lines);

        var formationsSummary = formations
            .Select(f => f.GetFormationSummary())
            .Sum();

        Console.WriteLine(formationsSummary);
    }

    static List<Formation> Parse(List<string> lines)
    {
        var formations = new List<Formation>();
        while (lines.Count > 0)
        {
            var emptyLineIndex = lines.IndexOf(string.Empty);
            if (emptyLineIndex == -1)
            {
                formations.Add(ParseFormation(lines)); // deal with last block
                lines.Clear();
                continue;
            }
            var subList = lines.GetRange(0, emptyLineIndex);
            lines.RemoveRange(0, emptyLineIndex + 1);
            formations.Add(ParseFormation(subList));
        }
        return formations;
    }

    static Formation ParseFormation(List<string> lines)
    {
        var positions = new HashSet<Position>();
        for (int r = 0; r < lines.Count; r++)
        {
            for (int c = 0; c < lines[r].Length; c++)
            {
                var character = lines[r][c];
                var type = character == '.'
                    ? ObjectType.Ash
                    : ObjectType.Rock;
                positions.Add(new Position(r, c, type));
            }
        }
        return new Formation(positions);
    }
}