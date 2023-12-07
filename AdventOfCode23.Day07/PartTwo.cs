using System.ComponentModel;

namespace AdventOfCode23.Day07;

static class SolutionDay07Part02
{
    static char[] cardTypes = { 'J', '2', '3', '4', '5', '6', '7', '8', '9', 'T', 'Q', 'K', 'A' };
    static Dictionary<char, int> cardValues = cardTypes
        .Zip(Enumerable.Range(0, cardTypes.Length), (type, value) => (type, value))
        .ToDictionary(t => t.type, t => t.value);
    
    class Hand : IComparable<Hand>
    {
        public enum HandType { HighCard = 0, OnePair, TwoPair, ThreeOfAKind, FullHouse, FourOfAKind, FiveOfAKind }
        List<char> _cards;
        public int Bid { get; init; }
        public HandType Type { get; init; }

        public Hand(List<char> cards, int bid)
        {
            _cards = cards;
            Bid = bid;
            Type = DetermineType();
        }

        private HandType DetermineType()
        {
            var type = 
                    IsFiveOfAKind() ? HandType.FiveOfAKind
                :   IsFourOfAKind() ? HandType.FourOfAKind
                :   IsFullHouse() ? HandType.FullHouse
                :   IsThreeOfAKind() ? HandType.ThreeOfAKind
                :   IsTwoPair() ? HandType.TwoPair
                :   IsOnePair() ? HandType.OnePair
                :   IsHighCard() ? HandType.HighCard
                :   throw new InvalidEnumArgumentException();

            if (_cards.Contains('J'))
            {
                type = AccountForJokers(type);
            }

            return type;
        }

        private HandType AccountForJokers(HandType type)
        {
            var jokerCount = _cards.Where(c => c == 'J').Count();
            return type switch
            {
                HandType.FiveOfAKind => HandType.FiveOfAKind,
                HandType.FourOfAKind => HandType.FiveOfAKind,
                HandType.FullHouse => HandType.FiveOfAKind,
                HandType.ThreeOfAKind => HandType.FourOfAKind,
                HandType.TwoPair => jokerCount == 1
                    ? HandType.FullHouse
                    : HandType.FourOfAKind,
                HandType.OnePair => HandType.ThreeOfAKind,
                HandType.HighCard => HandType.OnePair,
                _ => throw new InvalidEnumArgumentException()
            };
        }

        private IEnumerable<char[]> GetGroups()
        {
            return _cards
                .GroupBy(c => c)
                .Select(g => g.ToArray());
        }

        private bool IsFiveOfAKind()
        {
            return _cards.Distinct().Count() == 1;
        }

        private bool IsFourOfAKind()
        {
            var groups = GetGroups();
            return groups.Any(g => g.Length == 4);
        }

        private bool IsFullHouse()
        {
            var groups = GetGroups();
            return groups.Any(g => g.Length == 2)
                && groups.Any(g => g.Length == 3);
        }

        private bool IsThreeOfAKind()
        {
            var groups = GetGroups();
            return groups.Any(g => g.Length == 3)
                && groups.Any(g => g.Length == 1);
        }

        private bool IsTwoPair()
        {
            var groups = GetGroups();
            return groups
                .Where(g => g.Length == 2)
                .Count() == 2;
        }

        private bool IsOnePair()
        {
            return _cards.Distinct().Count() == 4;
        }

        private bool IsHighCard()
        {
            return _cards.Distinct().Count() == 5;
        }

        public int CompareTo(Hand? other)
        {
            if (other is null) return 1;

            if (Type != other.Type)
            {
                return Type.CompareTo(other.Type);
            }

            for (int i = 0; i < _cards.Count(); i++)
            {
                var value = cardValues[_cards[i]];
                var otherValue = cardValues[other._cards[i]];
                if (value != otherValue)
                {
                    return value.CompareTo(otherValue);
                }
            }

            return 0;
        }
    }
    public static void Solution()
    {
        var lines = File.ReadAllLines("input07.txt").ToList();
        var hands = Parse(lines);
        hands.Sort((a, b) => a.CompareTo(b));

        var totalWinnings = hands
            .Select((h, idx) => h.Bid * (idx + 1))
            .Sum();        

        Console.WriteLine(totalWinnings);
    }

    static List<Hand> Parse(List<string> lines)
    {
        var hands = new List<Hand>();
        foreach(var line in lines)
        {
            hands.Add(ParseHand(line));
        }
        return hands;
    }

    static Hand ParseHand(string line)
    {
        var parts = line.Split(" ");
        var cards = parts[0].ToList();
        var bid = int.Parse(parts[1]);

        return new Hand(cards, bid);
    }
}