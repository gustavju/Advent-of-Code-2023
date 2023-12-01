namespace AoC2023;

public class Day1 : IDay
{
    public string PartOne(string input) =>
         input.Lines()
            .Select(l => int.Parse($"{l.First(char.IsDigit)}{l.Last(char.IsDigit)}"))
            .Sum()
            .ToString();
    
    public string PartTwo(string input)
    {
        var numbers = new List<string> { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
        var sum = 0;
        foreach (var line in input.Lines())
        {
            var digits = new List<int>();
            for (int charIdx = 0; charIdx < line.Length; charIdx++)
            {
                if (char.IsDigit(line[charIdx]))
                {
                    digits.Add(int.Parse(line[charIdx].ToString()));
                }

                for (int numberIdx = 0; numberIdx < numbers.Count; numberIdx++)
                {
                    if (line[charIdx..].StartsWith(numbers[numberIdx]))
                    {
                        digits.Add(numberIdx + 1);
                    }
                }
            }
            var combination = $"{digits.First()}{digits.Last()}";
            sum += int.Parse(combination);
        }
        return sum.ToString();
    }
}