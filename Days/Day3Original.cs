namespace AoC2023;

public class Day3Original : IDay
{
    readonly List<(int, int)> adjacent =
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
            bool isNumber = false;
            var numberPos = new List<(int, int)>();
            var currentNum = "";
            for (int c = 0; c < matrix[0].Length; c++)
            {
                if (char.IsDigit(matrix[r][c]))
                {
                    isNumber = true;
                    numberPos.Add((r, c));
                    currentNum = currentNum + matrix[r][c];
                }
                else if (isNumber && !char.IsDigit(matrix[r][c]))
                {
                    isNumber = false;

                    foreach (var (nr, nc) in numberPos)
                    {
                        if (isNextTosym(nr, nc, matrix))
                        {
                            sum += int.Parse(currentNum);
                            break;
                        }
                    }

                    numberPos.Clear();
                    currentNum = "";
                }
            }
            if (isNumber)
            {
                foreach (var (nr, nc) in numberPos)
                {
                    if (isNextTosym(nr, nc, matrix))
                    {
                        sum += int.Parse(currentNum);
                        break;
                    }
                }
            }
        }

        return sum.ToString();
    }

    private bool isNextTosym(int r, int c, char[][] matrtix)
    {
        foreach (var (rr, cc) in adjacent)
        {

            if (
                r + rr < matrtix.Length && r + rr >= 0 &&
                c + cc < matrtix[0].Length && c + cc >= 0 &&
                matrtix[r + rr][c + cc] != '.' && !char.IsDigit(matrtix[r + rr][c + cc]))
            {
                return true;
            }
        }
        return false;
    }

    public string PartTwo(string input)
    {
        var sum = 0;
        var matrix = input.Lines().Select(l => l.ToCharArray()).ToArray();

        var symbols = new Dictionary<(int r, int c), char>();
        var numbers = new List<(int number, List<(int r, int c)> positions)>();

        for (int r = 0; r < matrix.Length; r++)
        {
            bool isNumber = false;
            var numberPos = new List<(int, int)>();
            var currentNum = "";
            for (int c = 0; c < matrix[0].Length; c++)
            {
                if (!char.IsDigit(matrix[r][c]) && matrix[r][c] != '.')
                    symbols.Add((r, c), matrix[r][c]);

                if (char.IsDigit(matrix[r][c]))
                {
                    isNumber = true;
                    numberPos.Add((r, c));
                    currentNum = currentNum + matrix[r][c];
                }
                else if (isNumber && !char.IsDigit(matrix[r][c]))
                {
                    foreach (var (nr, nc) in numberPos)
                    {
                        if (isNextTosym(nr, nc, matrix))
                        {
                            numbers.Add((int.Parse(currentNum), numberPos.ToList()));
                            break;
                        }
                    }

                    isNumber = false;
                    numberPos.Clear();
                    currentNum = "";
                }
            }
            if (isNumber)
            {
                foreach (var (nr, nc) in numberPos)
                {
                    if (isNextTosym(nr, nc, matrix))
                    {
                        numbers.Add((int.Parse(currentNum), numberPos.ToList()));
                        break;
                    }
                }
            }
        }

        foreach (var (number, postitions) in numbers)
        {
            Console.WriteLine(number);
            foreach (var (r, c) in postitions)
            {
                Console.Write(r + ":" + c + ", ");
            }
            Console.WriteLine();
        }

        foreach (var ((r, c), sym) in symbols)
        {
            Console.WriteLine($"{sym} - [{r},{c}]");
        }

        var gears = symbols.Where(kv => kv.Value == '*');
        foreach (var gear in gears)
        {
            var found = new HashSet<(int number, List<(int r, int c)> positions)>();
            var gPos = gear.Key;
            foreach (var adj in adjacent)
            {
                var rr = gPos.r + adj.Item1;
                var cc = gPos.c + adj.Item2;

                foreach (var item in numbers.Where(n => n.positions.Any(rc => rc.r == rr && rc.c == cc)).ToList())
                {
                    found.Add(item);
                }
            }
            if (found.Count == 2) {
                numbers.Remove(found.First());
                numbers.Remove(found.Last());
                Console.WriteLine(found.First().number * found.Last().number);
                sum += found.First().number * found.Last().number;
            }
        }
        //sum += numbers.Select(n => n.number).Sum();
        return sum.ToString();
    }
}
