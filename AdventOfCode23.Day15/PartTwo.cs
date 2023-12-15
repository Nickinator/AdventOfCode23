using System.Text.RegularExpressions;

namespace AdventOfCode23.Day15;

static class SolutionDay15Part02
{
    enum Operation { Removal = 0, Insertion }
    record Instruction(string Label, Operation Operation, int BoxID, int FocalLength = 0);
    record Lens(string Label, int FocalLength)
    {
        public int FocalLength { get; set; } = FocalLength;
    }
    class Library
    {
        readonly Dictionary<int, List<Lens>> _boxes = new();

        public Library(IEnumerable<Instruction> instructions)
        {
            InitBoxes();
            foreach (var instruction in instructions)
            {
                HandleInstruction(instruction);
            }
        }

        private void InitBoxes()
        {
            for (int i = 0; i < 256; i++)
            {
                _boxes.Add(i, new());
            }
        }

        private void HandleInstruction(Instruction instruction)
        {
            var box = _boxes[instruction.BoxID];
            if (instruction.Operation is Operation.Removal)
            {
                HandleRemoval(instruction, box);
            }
            else
            {
                HandleInsertion(instruction, box);
            }
        }

        private static void HandleRemoval(Instruction instruction, List<Lens> box)
        {
            var lens = box.Where(l => l.Label == instruction.Label).FirstOrDefault();
            if (lens is not null)
            {
                box.Remove(lens);
            }
        }

        private static void HandleInsertion(Instruction instruction, List<Lens> box)
        {
            var lens = box.Where(l => l.Label == instruction.Label).FirstOrDefault();
            if (lens is not null)
            {
                var index = box.FindIndex(l => l == lens);
                box[index].FocalLength = instruction.FocalLength;
            }
            else
            {
                box.Add(new Lens(instruction.Label, instruction.FocalLength));
            }
        }

        public int TotalFocusingPower()
        {
            return _boxes
                .Select(pair => BoxFocusingPower(pair.Key, pair.Value))
                .Sum();
        }

        private static int BoxFocusingPower(int boxId, List<Lens> box)
        {
            return box
                .Select((lens, index) =>
                    (boxId + 1) *
                    (index + 1) *
                    lens.FocalLength)
                .Sum();
        }
    }

    public static void Solution()
    {
        var input = File.ReadAllText("input15.txt");
        var instructions = Parse(input);
        var library = new Library(instructions);

        var totalFocussingPower = library.TotalFocusingPower();
        Console.WriteLine(totalFocussingPower);
    }

    private static IEnumerable<Instruction> Parse(string input)
    {
        var instructionStrings = input.Split(',');
        foreach (var instruction in instructionStrings)
        {
            yield return ParseInstruction(instruction);
        }
    }

    private static Instruction ParseInstruction(string instruction)
    {
        var regex = new Regex(@"([a-z]+?)([=-])(\d)?");
        var match = regex.Match(instruction);
        var label = match.Groups[1].Value;
        var boxId = Hash(label);
        var operation = match.Groups[2].Value is "-"
            ? Operation.Removal
            : Operation.Insertion;
            
        var focalLength = operation is Operation.Insertion
            ? int.Parse(match.Groups[3].Value)
            : 0;

        return new Instruction(label, operation, boxId, focalLength);
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