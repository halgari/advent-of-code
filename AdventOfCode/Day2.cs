using FluentAssertions;

namespace AdventOfCode;

public class Day2
{
    [Fact]
    public void Day2_1()
    {
        var lines = GetLines();

        var key = new Dictionary<string, int>()
        {
            { "red", 12 },
            { "green", 13 },
            { "blue", 14 }
        };

        var possible = lines.Where(line => line.maxUsed.All(pair => pair.Value <= key[pair.Key]))
            .Sum(line => line.game);


        possible.Should().Be(2541);
    }

    [Fact]
    public void Day2_2()
    {
        var lines = GetLines();

        var key = new Dictionary<string, int>()
        {
            { "red", 12 },
            { "green", 13 },
            { "blue", 14 }
        };

        var possible = lines.Select(line =>
        {
            var power = line.maxUsed.GroupBy(pair => pair.Key)
                .Select(group => group.Max(pair => pair.Value))
                .Aggregate(1, (a, b) => a * b);
            return power;
        });
        
        possible.Sum().Should().Be(66016);
    }

    private (int game, Dictionary<string, int> maxUsed)[] GetLines()
    {
        // Format is: Game 1: X color, Y color, Z color
        var lines = GetLines(1)
            .Select(line => line.Split(' '))
            .Select(parts =>
            {
                var game = int.Parse(parts[1].Trim(':'));
                var colorParts = parts.Skip(2).Select(color => color.Trim(',').Trim(';')).ToArray();

                var maxUsed = new Dictionary<string, int>();
                for (var i = 0; i < colorParts.Length; i += 2)
                {
                    var used = int.Parse(colorParts[i]);
                    var color = colorParts[i + 1];
                    maxUsed[color] = Math.Max(maxUsed.GetValueOrDefault(color), used);
                }
                return (game, maxUsed);
            }).ToArray();
        return lines;
    }

    private IEnumerable<string> GetLines(int subDay)
    {
        using var stream = GetType().Assembly.GetManifestResourceStream($"AdventOfCode.Resources.Day2_{subDay}_input.txt");
        using var reader = new StreamReader(stream!);
        // Read all lines, trim them and remove empty lines
        var lines = reader.ReadToEnd().Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
            .Select(line => line.Trim())
            .Where(line => !string.IsNullOrEmpty(line));
        return lines;
    }
}