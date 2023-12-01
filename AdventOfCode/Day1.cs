using System.Reflection;
using FluentAssertions;

namespace AdventOfCode;

public class Day1
{
    [Fact]
    public void Day1_1()
    {
        var digits = GetLines(1)
            .Select(line => line.Where(char.IsDigit).ToArray())
            .Select(digits => digits.First().ToString() + digits.Last())
            .Select(digits => int.Parse(digits.ToString()));

        digits.Sum().Should().Be(56108);

    }

    /// <summary>
    /// Load the lines from the embedded resource
    /// </summary>
    /// <param name="subDay"></param>
    /// <returns></returns>
    private IEnumerable<string> GetLines(int subDay)
    {
        using var stream = GetType().Assembly.GetManifestResourceStream($"AdventOfCode.Resources.Day1_{subDay}_input.txt");
        using var reader = new StreamReader(stream!);
        // Read all lines, trim them and remove empty lines
        var lines = reader.ReadToEnd().Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
            .Select(line => line.Trim())
            .Where(line => !string.IsNullOrEmpty(line));
        return lines;
    }

    [Fact]
    public void Day1_2()
    {
        // Create a lookup table from english names to numbers (index + 1 is the number)
        var digits = new[]
        {
            "one",
            "two",
            "three",
            "four",
            "five",
            "six",
            "seven",
            "eight",
            "nine"
        };
        
        // Create a lookup table from english names to numbers
        var lookup = digits.Select((digit, index) => (digit, index))
            .ToDictionary(pair => pair.digit, pair => pair.index + 1);
        
        // Add the numerical digits as well
        for (var i = 0; i <= 9; i++)
        {
            lookup.Add(i.ToString(), i);
        }
     
        // Get the number for a single line
        int GetNumber(string line)
        {
            (int Idx, int Number) firstNumber = (int.MaxValue, -1);
            (int Idx, int Number) secondNumber = (-1, -1);
            
            // For each value in the lookup table
            foreach (var (digit, value) in lookup)
            {
                // Find it from the start
                var firstIdx = line.IndexOf(digit, StringComparison.InvariantCultureIgnoreCase);
                
                // If it was found and it's before the first number, update the first number
                if (firstIdx != -1 && firstIdx < firstNumber.Idx)
                {
                    firstNumber = (firstIdx, value);
                }
                
                // Find it from the end
                var lastIdx = line.LastIndexOf(digit, StringComparison.InvariantCultureIgnoreCase);
                
                // If it was found and it's after the second number, update the second number
                if (lastIdx != -1 && lastIdx > secondNumber.Idx)
                {
                    secondNumber = (lastIdx, value);
                }
            }
            
            // Return the number
            return (firstNumber.Number * 10) + secondNumber.Number;
        }
        
        var result = GetLines(2)
            // Use parallel processing, just for the fun of it
            .AsParallel()
            .Select(GetNumber)
            .Sum();
        
        result.Should().Be(55652);
    }
}