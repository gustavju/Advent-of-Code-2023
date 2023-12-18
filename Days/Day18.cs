namespace AoC2023;

public class Day18 : IDay
{
    enum Direction { East = 0, South = 1, West = 2, North = 3 };

    record Coord(long X, long Y);

    static Coord MoveDirection(Coord current, Direction direction, long distance) => direction switch
    {
        Direction.North => new Coord(current.X, current.Y + distance),
        Direction.East => new Coord(current.X + distance, current.Y),
        Direction.South => new Coord(current.X, current.Y - distance),
        Direction.West => new Coord(current.X - distance, current.Y),
        _ => throw new ArgumentException()
    };

    static Direction ParseDirection(char c) => c switch
    {
        'U' => Direction.North,
        'D' => Direction.South,
        'L' => Direction.West,
        'R' => Direction.East,
        _ => throw new ArgumentException()
    };

    private static long ShoelaceAlgorithm(IEnumerable<(long dist, Direction dir)> steps)
    {
        List<Coord> points = [];
        Coord currentLocation = new(0, 0);

        long perimiter = 0;
        foreach ((var dist, var dir) in steps)
        {
            perimiter += dist;
            currentLocation = MoveDirection(currentLocation, dir, dist);
            points.Add(currentLocation);
        }

        long leftSum = 0;
        long rightSum = 0;
        for (int i = 0; i < points.Count; i++)
        {
            leftSum += points[i].X * points[(i + 1) % points.Count].Y;
            rightSum += points[i].Y * points[(i + 1) % points.Count].X;
        }

        return (perimiter / 2) + Math.Abs((leftSum - rightSum) / 2) + 1;
    }

    public string PartOne(string input)
    {
        var steps = input.Lines().Select(l =>
        {
            var w = l.Words();
            return (long.Parse(w.ElementAt(1)), ParseDirection(w.First()[0]));
        });
        return ShoelaceAlgorithm(steps).ToString();
    }

    public string PartTwo(string input)
    {
        var steps = input.Lines().Select(l =>
        {
            var hex = l.Words().ElementAt(2).TrimStart('(', '#').TrimEnd(')');
            var direction = (Direction)(hex[^1] - '0');
            return (long.Parse(hex[..5], System.Globalization.NumberStyles.HexNumber), direction);
        });
        return ShoelaceAlgorithm(steps).ToString();
    }
}
