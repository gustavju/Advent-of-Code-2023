namespace AoC2023;

public class Day2 : IDay
{
    record Game
    {
        public int id;
        public required IEnumerable<IEnumerable<(int cubeCnt, string color)>> matches;
    }

    private static Game ParseGame(string line)
    {
        var gameParts = line.Split(": ");
        var gameId = gameParts.First().Ints().First();
        var subGames = gameParts[1].Split("; ");
        return new Game()
        {
            id = gameId,
            matches = subGames
                .Select(subGame => subGame.Split(", ")
                    .Select(s => (int.Parse(s.Split(" ").First()), s.Split(" ").Last())))
        };
    }

    public string PartOne(string input)
    {
        const int maxRed = 12, maxBlue = 14, maxGreen = 13;
        return input.Lines().Select(ParseGame).Sum(game =>
        {
            foreach (var match in game.matches)
            {
                var dict = new Dictionary<string, int>();
                foreach ((int cubeCnt, string cubeColor) in match)
                {
                    if (!dict.TryAdd(cubeColor, cubeCnt))
                    {
                        dict[cubeColor] += cubeCnt;
                    }
                }
                if (dict.TryGetValue("red", out int reds) && reds > maxRed ||
                    dict.TryGetValue("green", out int greens) && greens > maxGreen ||
                    dict.TryGetValue("blue", out int blues) && blues > maxBlue)
                {
                    return 0;
                }
            }
            return game.id;
        }).ToString();
    }

    public string PartTwo(string input) => 
        input.Lines().Select(ParseGame).Sum(game =>
        {
            var maxCntByColor = new Dictionary<string, int>();
            foreach (var match in game.matches)
            {
                foreach ((int cubeCnt, string cubeColor) in match)
                {
                    if (!maxCntByColor.TryAdd(cubeColor, cubeCnt) &&
                        cubeCnt > maxCntByColor[cubeColor])
                    {
                        maxCntByColor[cubeColor] = cubeCnt;
                    }
                }
            }
            return maxCntByColor["red"] * maxCntByColor["green"] * maxCntByColor["blue"];
        }).ToString();
}
