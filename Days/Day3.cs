namespace AoC2023;

public class Day3 : IDay
{
    readonly List<(int r, int c)> adjacent =
    [
        (-1,-1), (-1,0), (-1,1),
        (0,-1),          (0,1),
        (1,-1),  (1,0),  (1,1),
    ];
    public string PartOne(string input)
    {
        var sum = 0;
        var matrix = input.Lines().Select(l => l.ToCharArray()).ToArray();

        for (int r = 0; r < matrix.Length; r++)
        {
            bool isNextToSymbol = false;
            var currentNum = 0;
            for (int c = 0; c < matrix[0].Length; c++)
            {
                if (char.IsDigit(matrix[r][c]))
                {
                    currentNum = 10 * currentNum + (matrix[r][c] - '0');
                    isNextToSymbol = isNextToSymbol || IsNextToSymbol(r, c, matrix);
                }
                else if (currentNum != 0)
                {
                    if (isNextToSymbol)
                    {
                        sum += currentNum;
                    }
                    isNextToSymbol = false;
                    currentNum = 0;
                }
            }
            if (currentNum != 0 && isNextToSymbol)
            {
                sum += currentNum;
            }
        }
        return sum.ToString();
    }

    private bool IsNextToSymbol(int r, int c, char[][] matrix)
    {
        foreach (var (ar, ac) in adjacent)
        {
            var rr = r + ar;
            var cc = c + ac;
            if (matrix.IsInBounds(rr, cc) &&
                matrix[rr][cc] != '.' &&
                !char.IsDigit(matrix[rr][cc]))
            {
                return true;
            }
        }
        return false;
    }

    private (int r, int c)? GearNextToNumber(int r, int c, char[][] matrix)
    {
        foreach (var (ar, ac) in adjacent)
        {
            var rr = r + ar;
            var cc = c + ac;
            if (matrix.IsInBounds(rr, cc) && matrix[rr][cc] == '*')
            {
                return (rr, cc);
            }
        }
        return null;
    }

    public string PartTwo(string input)
    {
        var sum = 0;
        var matrix = input.Lines().Select(l => l.ToCharArray()).ToArray();
        var gearsToNumbers = new Dictionary<(int r, int c), List<int>>();

        for (int r = 0; r < matrix.Length; r++)
        {
            (int r, int c)? gear = null;
            var currentNum = 0;
            for (int c = 0; c < matrix[0].Length; c++)
            {
                if (char.IsDigit(matrix[r][c]))
                {
                    currentNum = 10 * currentNum + (matrix[r][c] - '0');
                    gear ??= GearNextToNumber(r, c, matrix);
                }
                else if (currentNum != 0)
                {
                    if (gear.HasValue)
                    {
                        if (!gearsToNumbers.TryAdd(gear.Value, [currentNum]))
                            gearsToNumbers[gear.Value].Add(currentNum);
                    }
                    gear = null;
                    currentNum = 0;
                }
            }
            if (currentNum != 0 && gear.HasValue)
            {
                if (!gearsToNumbers.TryAdd(gear.Value, [currentNum]))
                    gearsToNumbers[gear.Value].Add(currentNum);
            }
        }

        foreach (var gearToNumbers in gearsToNumbers.Where(g => g.Value.Count == 2))
        {
            sum += gearToNumbers.Value.First() * gearToNumbers.Value.Last();
        }
        return sum.ToString();
    }
}