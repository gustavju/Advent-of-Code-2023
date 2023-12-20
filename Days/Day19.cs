namespace AoC2023;

public class Day19 : IDay
{
    readonly struct Entry
    {
        public Entry(string str)
        {
            var i = str.Ints().ToArray();
            x = i[0];
            m = i[1];
            a = i[2];
            s = i[3];
        }
        public readonly int GetEntryValueByName(string str) => str switch
        {
            "x" => x,
            "m" => m,
            "a" => a,
            "s" => s,
            _ => throw new ArgumentException(str)
        };
        public int x { get; }
        public int m { get; }
        public int a { get; }
        public int s { get; }
    }

    struct Rule
    {
        public Rule(string rule)
        {
            var splitRule = rule.Split('<', '>');
            if (splitRule.Length == 2)
            {
                var right = splitRule[1].Split(':');

                VarName = splitRule[0];
                Value = int.Parse(right[0]);
                NextRule = right[1];
                Operand = rule[1];
            }
            else
            {
                NextRule = rule;
            }
        }
        public string VarName { get; set; } = string.Empty;
        public char Operand { get; set; }
        public int Value { get; set; }
        public string NextRule { get; set; }
    }
    struct Flow
    {
        public Flow(string s)
        {
            Name = s.Split('{')[0];
            var rules = s.Split('{')[1].TrimEnd('}').Split(',').ToList();
            foreach (var rule in rules)
            {
                Rules.Add(new Rule(rule));
            }
        }
        public string Name { get; set; }
        public List<Rule> Rules { get; set; } = new();
    }

    private bool IsOK(Dictionary<string, Flow> flows, Entry entry)
    {
        var currentRule = "in";
        while (true)
        {
            foreach (var rule in flows[currentRule].Rules)
            {
                var ok = true;
                if (!String.IsNullOrEmpty(rule.VarName))
                {
                    ok = rule.Operand == '>' ?
                        entry.GetEntryValueByName(rule.VarName) > rule.Value :
                        entry.GetEntryValueByName(rule.VarName) < rule.Value;
                }
                if (ok)
                {
                    if (rule.NextRule == "A")
                        return true;
                    if (rule.NextRule == "R")
                        return false;
                    currentRule = rule.NextRule;
                    break;
                }
            }
        }
    }

    public string PartOne(string input)
    {
        var sections = input.Split($"{Environment.NewLine}{Environment.NewLine}");
        var flows = sections[0].Lines().Select(l => new Flow(l)).ToDictionary(f => f.Name);
        var entries = sections[1].Lines().Select(l => new Entry(l));

        return entries.Where(e => IsOK(flows, e)).Sum(e => e.x + e.m + e.a + e.s).ToString();
    }

    Dictionary<string, Flow>? flows;
    public string PartTwo(string input)
    {
        var sections = input.Split($"{Environment.NewLine}{Environment.NewLine}");
        flows = sections[0].Lines().Select(l => new Flow(l)).ToDictionary(f => f.Name);
        var candidates = new Dictionary<string, (int Min, int Max)>
        {
            ["x"] = (1, 4000),
            ["m"] = (1, 4000),
            ["a"] = (1, 4000),
            ["s"] = (1, 4000)
        };
        return CalcRanges("in", candidates).ToString();
    }

    long CalcRanges(string currentRule, Dictionary<string, (int Min, int Max)> ranges)
    {
        if (currentRule == "A")
            return ranges.Values
                    .Aggregate<(int Min, int Max), long>(1, (current, range) => current * (range.Max - range.Min + 1));
        if (currentRule == "R")
            return 0;

        long result = 0;
        
        #pragma warning disable CS8602 // Dereference of a possibly null reference.
        foreach (var rule in flows[currentRule].Rules)
        {
            var (min, max) = string.IsNullOrEmpty(rule.VarName) ? (0, 0) : ranges[rule.VarName];
            if (rule.Operand == '<')
            {
                if (max < rule.Value)
                    return result + CalcRanges(rule.NextRule, ranges);

                if (min < rule.Value)
                {
                    var newRanges = new Dictionary<string, (int Min, int Max)>(ranges)
                    {
                        [rule.VarName] = (min, rule.Value - 1)
                    };
                    result += CalcRanges(rule.NextRule, newRanges);

                    ranges[rule.VarName] = (rule.Value, max);
                }
            }
            else if (rule.Operand == '>')
            {
                if (min > rule.Value)
                    return result + CalcRanges(rule.NextRule, ranges);

                if (max > rule.Value)
                {
                    var newRanges = new Dictionary<string, (int Min, int Max)>(ranges)
                    {
                        [rule.VarName] = (rule.Value + 1, max)
                    };
                    result += CalcRanges(rule.NextRule, newRanges);

                    ranges[rule.VarName] = (min, rule.Value);
                }
            }
            else
            {
                result += CalcRanges(rule.NextRule, ranges);
            }
        }
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        return result;
    }
}
