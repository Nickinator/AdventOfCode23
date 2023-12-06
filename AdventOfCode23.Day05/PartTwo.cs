using System.Text.RegularExpressions;

namespace AdventOfCode23.Day05;

static class SolutionDay05Part02
{
    record SeedRange(long Start, long Length)
    {
        public bool ContainsValue(long value)
        {
            return value >= Start && value <= Start + Length;
        }
    }
    record Range(long Dest, long Src, long Length)
    {
        public bool ContainsValue(long candidate)
        {
            return candidate >= Dest && candidate <= Dest + Length;
        }

        public long GetKey(long value)
        {
            var offset = value - Dest;
            return Src + offset;
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

        private long ReverseLookup(long value, List<Range> map)
        {
            var range = map.Where(r => r.ContainsValue(value));
            if (!range.Any())
            {
                return value;
            }
            else
            {
                return range.ElementAt(0).GetKey(value);
            }
        }

        public long GetMinLocation()
        {
            var location = 0;
            while (true)
            {
                var humidity = ReverseLookup(location, _humidityToLocation);
                var temperature = ReverseLookup(humidity, _temperatureToHumidity);
                var light = ReverseLookup(temperature, _lightToTemperature);
                var water = ReverseLookup(light, _waterToLight);
                var fertilizer = ReverseLookup(water, _fertilizerToWater);
                var soil = ReverseLookup(fertilizer, _soilToFertilizer);
                var seed = ReverseLookup(soil, _seedToSoil);

                if (_seeds.Any(s => s.ContainsValue(seed)))
                {
                    return location;
                }

                location++;
            }
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