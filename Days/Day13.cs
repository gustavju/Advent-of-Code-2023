namespace AoC2023;

public class Day13 : IDay
{
    enum Direction { Horizontal, Vertical };
    private static int FindMirrorLines(char[][] currrentGrid, int smudgeFactor, Direction direction)
    {
        int ans = 0;
        int currentBound = direction == Direction.Horizontal ? currrentGrid[0].Length : currrentGrid.Length;
        int oppositeDirBound = direction == Direction.Horizontal ? currrentGrid.Length : currrentGrid[0].Length;
        for (int outer = 0; outer < currentBound - 1; outer++)
        {
            int smudgeCount = 0;
            for (int inner = 0; inner < currentBound; inner++)
            {
                var before = outer - inner;
                var after = outer + 1 + inner;
                if (0 <= before && before < after && after < currentBound)
                {
                    for (int c = 0; c < oppositeDirBound; c++)
                    {
                        if ((direction == Direction.Horizontal && currrentGrid[c][before] != currrentGrid[c][after]) ||
                            (direction == Direction.Vertical && currrentGrid[before][c] != currrentGrid[after][c]))
                            smudgeCount++;
                    }
                }
            }
            if (smudgeCount == smudgeFactor)
                ans += direction == Direction.Horizontal ? outer + 1 : 100 * (outer + 1);
        }
        return ans;
    }
    private static int FindMirrorsWithSmudgeFactor(string input, int smudgeFactor = 0)
    {
        var gridStrings = input.Split(Environment.NewLine + Environment.NewLine);
        int ans = 0;
        foreach (var gridString in gridStrings)
        {
            var grid = gridString.Lines().Select(r => r.ToCharArray()).ToArray();
            ans += FindMirrorLines(grid, smudgeFactor, Direction.Horizontal);
            ans += FindMirrorLines(grid, smudgeFactor, Direction.Vertical);
        }
        return ans;
    }

    public string PartOne(string input) => FindMirrorsWithSmudgeFactor(input, 0).ToString();

    public string PartTwo(string input) => FindMirrorsWithSmudgeFactor(input, 1).ToString();
}