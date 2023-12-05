using System.Runtime.Versioning;
using System.Text.RegularExpressions;

namespace AdventOfCode23.Day05;

static class SolutionDay05Part01
{
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
        List<long> _seeds = new();
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
            _seeds = matches
                .Select(m => long.Parse(m.Value))
                .ToList();
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

        public IEnumerable<long> GetLocations()
        {
            foreach (var seed in _seeds)
            {
                var soil = Lookup(seed, _seedToSoil);
                var fertilizer = Lookup(soil, _soilToFertilizer);
                var water = Lookup(fertilizer, _fertilizerToWater);
                var light = Lookup(water, _waterToLight);
                var temperature = Lookup(light, _lightToTemperature);
                var humidity = Lookup(temperature, _temperatureToHumidity);
                var location = Lookup(humidity, _humidityToLocation);
                yield return location;
            }
        }
    }

    public static void Solution()
    {
        var inputString = File.ReadAllText("input05.txt");
        var mapper = new SeedMapper(inputString);
        var minLocation = mapper
            .GetLocations()
            .Min();

        Console.WriteLine(minLocation);
    }
}