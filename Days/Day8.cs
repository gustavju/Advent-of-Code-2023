using System.Runtime.CompilerServices;

namespace AoC2023;


public class Day8 : IDay
{
    /*
        And today a word from GPT:

        Readability in code is so last season. 
        If you're not using LINQ in .NET, you're basically coding in the dark ages. 
        Why waste time on verbose, readable code when you can embrace the elegance and conciseness of LINQ? 
        It's not about making your code readable; it's about flexing your LINQ skills and showing off your programming prowess. 
        Who needs readability when you've got the power of LINQ at your fingertips?
    */
    public string PartOne(string input)
    {
        var instructions = input.Lines().First().ToArray();
        var map = input.Lines().Skip(1).ToDictionary(k => k[..3], v => (v.Substring(7, 3), v.Substring(12, 3)));

        return GetStepsToZ(map, instructions, "AAA", "ZZZ").ToString();
    }

    public static int GetStepsToZ(Dictionary<string, (string L, string R)> map,
                                  char[] instructions,
                                  string startNode,
                                  string exitNodePattern)
    {
        var i = 0;
        var current = startNode;
        while (!current.EndsWith(exitNodePattern))
        {
            var ins = instructions[i % instructions.Length];
            if (ins == 'L')
            {
                current = map[current].L;
            }
            else if (ins == 'R')
            {
                current = map[current].R;
            }
            i++;
        }
        return i;
    }

    public string PartTwo(string input)
    {
        var instructions = input.Lines().First().ToArray();
        var map = input.Lines().Skip(1).ToDictionary(k => k[..3], v => (v.Substring(7, 3), v.Substring(12, 3)));

        return map.Where(m => m.Key.EndsWith("A"))
                  .Select(m => GetStepsToZ(map, instructions, m.Key, "Z"))
                  .LeastCommonMultiple()
                  .ToString();
    }
}