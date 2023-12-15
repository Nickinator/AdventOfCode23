namespace AdventOfCode23.Day15;

static class SolutionDay15Part01
{
    public static void Solution()
    {
        var input = File.ReadAllText("input15.txt");
        var steps = input.Split(',');

        var sumOfHashes = steps
            .Select(s => Hash(s))
            .Sum();
        Console.WriteLine(sumOfHashes);
    }

    private static int Hash(string word)
    {
        var currentValue = 0;
        foreach (char c in word)
        {
            int ascii = (int)c;
            currentValue += ascii;
            currentValue *= 17;
            currentValue %= 256;
        }

        return currentValue;
    }
}