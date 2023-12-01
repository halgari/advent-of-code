using System.Reflection;
using FluentAssertions;

namespace AdventOfCode;

public class Day1
{
    [Fact]
    public void Day1_1()
    {
        using var stream = GetType().Assembly.GetManifestResourceStream("AdventOfCode.Resources.Day1_1_input.txt");
        using var reader = new StreamReader(stream);
        
        var lines = reader.ReadToEnd().Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
            .Select(line => line.Trim())
            .Where(line => !string.IsNullOrEmpty(line));
        
        var digits = lines.Select(line => line.Where(char.IsDigit).ToArray())
            .Select(digits => digits.First().ToString() + digits.Last())
            .Select(digits => int.Parse(digits.ToString()))
            .ToArray();

        digits.Sum().Should().Be(56108);

    }
}