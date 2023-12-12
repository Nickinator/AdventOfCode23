using System.Text.RegularExpressions;

namespace AdventOfCode23.Day12;

static class SolutionDay12Part01
{
    class ConditionRecord
    {
        string _springs = null!;
        List<int> _damagedGroups = new();

        public ConditionRecord(string springs, IEnumerable<int> groups)
        {
            _springs = springs;
            _damagedGroups = groups.ToList();
        }

        private bool IsValidArrangement(string springs)
        {
            var parts = springs
                .Split('.')
                .Where(p => string.IsNullOrEmpty(p) is false);
            var groupLengths = parts.Select(p => p.Length);
            return _damagedGroups.SequenceEqual(groupLengths);
        }

        public int CalculateValidArrangementsCount()
        {
            var permutations = new List<string>();
            int startIndex = 0;
            GeneratePermutations(_springs.ToCharArray(), startIndex, permutations);

            return permutations
                .Where(p => IsValidArrangement(p))
                .Count();
        }

        private void GeneratePermutations(char[] chars, int index, List<string> permutations)
        {
            if (index == chars.Length)
            {
                permutations.Add(new string(chars));
                return;
            }

            if (chars[index] == '?')
            {
                chars[index] = '#';
                GeneratePermutations(chars, index + 1, permutations);

                chars[index] = '.';
                GeneratePermutations(chars, index + 1, permutations);

                chars[index] = '?'; // reset
            }

            else
            {
                GeneratePermutations(chars, index + 1, permutations);
            }
        }
    }
    public static void Solution()
    {
        var lines = File.ReadAllLines("input12.txt").ToList();
        var conditionRecords = Parse(lines).ToList();
        
        var sumOfArrangements = conditionRecords
            .Select(r => r.CalculateValidArrangementsCount())
            .Sum();
        Console.WriteLine(sumOfArrangements);
    }

    static IEnumerable<ConditionRecord> Parse(List<string> lines)
    {
        foreach (var line in lines)
        {
            yield return ParseRecord(line);
        }
    }

    static ConditionRecord ParseRecord(string line)
    {
        var parts = line.Split(" ");
        var springs = parts[0];
        var groupString = parts[1];
        var regex = new Regex(@"\d+");
        var matches = regex.Matches(groupString);
        var groups = matches.Select(m => int.Parse(m.Value));

        return new ConditionRecord(springs, groups);
    }
}