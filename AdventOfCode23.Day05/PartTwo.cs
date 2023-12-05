using System.Text.RegularExpressions;

namespace AdventOfCode23.Day05;

static class SolutionDay05Part02
{
    record SeedRange(long Start, long Length);
    record Range(long Dest, long Src, long Length)
    {
        public bool ContainsKey(long candidate)
        {
            return candidate >= Src && candidate <= Src + Length;
        }

        public long GetValue(long key)
        {
            var offset = key - Src;
            return Dest + offset;
        }
    }
    class SeedMapper
    {
        List<SeedRange> _seeds = new();
        List<Range> _seedToSoil = new();
        List<Range> _soilToFertilizer = new();
        List<Range> _fertilizerToWater = new();
        List<Range> _waterToLight = new();
        List<Range> _lightToTemperature = new();
        List<Range> _temperatureToHumidity = new();
        List<Range> _humidityToLocation = new();

        public SeedMapper(string input)
        {
            Parse(input);
        }

        private void Parse(string input)
        {
            var sections = input.Split("\r\n\r\n");
            ParseSeeds(sections[0]);
            _seedToSoil = ParseMap(sections[1]);
            _soilToFertilizer = ParseMap(sections[2]);
            _fertilizerToWater = ParseMap(sections[3]);
            _waterToLight = ParseMap(sections[4]);
            _lightToTemperature = ParseMap(sections[5]);
            _temperatureToHumidity = ParseMap(sections[6]);
            _humidityToLocation = ParseMap(sections[7]);
        }

        private void ParseSeeds(string seedString)
        {
            var pattern = @"\d+";
            var regex = new Regex(pattern);
            var matches = regex.Matches(seedString);
            var numbers = matches.Select(m => long.Parse(m.Value));
            for (int i = 0; i < numbers.Count(); i += 2)
            {
                _seeds.Add(new SeedRange(
                    numbers.ElementAt(i),
                    numbers.ElementAt(i + 1)));
            }
        }

        private List<Range> ParseMap(string mapString)
        {
            var map = new List<Range>();
            var pattern = @"\d+";
            var regex = new Regex(pattern);
            var numberStrings = mapString.Split("\r\n").Skip(1);
            foreach (var numberLine in numberStrings)
            {
                var numbers = regex.Matches(numberLine).Select(m => long.Parse(m.Value));
                (long dst, long src, long length) = (numbers.ElementAt(0), numbers.ElementAt(1), numbers.ElementAt(2));
                map.Add(new Range(dst, src, length));
            }

            return map;
        }

        private long Lookup(long key, List<Range> map)
        {
            var range = map.Where(r => r.ContainsKey(key));
            if (!range.Any())
            {
                return key;
            }
            else
            {
                return range.ElementAt(0).GetValue(key);
            }
        }

        public long GetMinLocation()
        {
            var min = long.MaxValue;
            foreach (var seed in _seeds)
            {
                Console.WriteLine(seed.Length);
                for (long i = seed.Start; i < seed.Start + seed.Length; i++)
                {
                    var soil = Lookup(i, _seedToSoil);
                    var fertilizer = Lookup(soil, _soilToFertilizer);
                    var water = Lookup(fertilizer, _fertilizerToWater);
                    var light = Lookup(water, _waterToLight);
                    var temperature = Lookup(light, _lightToTemperature);
                    var humidity = Lookup(temperature, _temperatureToHumidity);
                    var location = Lookup(humidity, _humidityToLocation);
                    if (location < min)
                    {
                        min = location;
                    }
                }
            }

            return min;
        }
    }

    public static void Solution()
    {
        var inputString = File.ReadAllText("input05.txt");
        var mapper = new SeedMapper(inputString);
        var minLocation = mapper.GetMinLocation();

        Console.WriteLine(minLocation);
    }
}