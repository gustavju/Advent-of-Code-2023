namespace AoC2023;

public class Day20 : IDay
{
    enum ModuleType { Conjunction, FlipFlop, Broadcaster };
    enum PulseType { High, Low }
    class Module
    {
        public Module(string str)
        {
            var sections = str.Split(" -> ");
            if (str[0] == '%')
            {
                Id = sections[0][1..];
                Type = ModuleType.FlipFlop;
                PulseType = PulseType.Low;
            }
            else if (str[0] == '&')
            {
                Id = sections[0][1..];
                Type = ModuleType.Conjunction;
            }
            else
            {
                Id = sections[0];
                Type = ModuleType.Broadcaster;
            }
            Destinations = sections[1].Split(",").Select(d => d.Trim()).ToList();
        }
        public string Id { get; set; }
        public PulseType PulseType { get; set; }
        public ModuleType Type { get; set; }
        public List<string> Destinations { get; set; } = [];
        public Dictionary<string, PulseType> Inputs { get; set; } = [];
        public List<(string, string, PulseType)> HandlePulse(string from, PulseType incomingPulseType)
        {
            List<(string, string, PulseType)> toQueue = [];
            var outgoingPulseType = incomingPulseType;
            switch (Type)
            {
                case ModuleType.Broadcaster: // pass incoming pulse along
                    break;
                case ModuleType.FlipFlop:
                    if (incomingPulseType == PulseType.High)
                        return toQueue; // ignore

                    PulseType = PulseType == PulseType.High ? PulseType.Low : PulseType.High;
                    outgoingPulseType = PulseType;
                    break;
                case ModuleType.Conjunction:
                    Inputs[from] = incomingPulseType;
                    outgoingPulseType = Inputs.Values.All(v => v == PulseType.High) ? PulseType.Low : PulseType.High;
                    break;
            }
            return Destinations.Select(d => (d, Id, outgoingPulseType)).ToList();
        }
    }

    static Dictionary<string, Module> ParseAndLinkModules(string input)
    {
        var modules = input.Lines().Select(l => new Module(l)).ToDictionary(m => m.Id);
        foreach (var conModule in modules.Values.Where(m => m.Type == ModuleType.Conjunction))
        {
            conModule.Inputs = modules.Values.Where(m => m.Destinations.Contains(conModule.Id)).ToDictionary(k => k.Id, v => PulseType.Low);
        }
        return modules;
    }

    static long PulseNetwork(string input, bool part1 = true)
    {
        var modules = ParseAndLinkModules(input);
        var secondToLastId = modules.Values.Where(m => m.Destinations.Contains("rx")).Single().Id;
        var thirdToLast = modules.Values.Where(m => m.Destinations.Contains(secondToLastId)).Select(m => m.Id).ToDictionary(k => k, v => 0);
        // (&kd + &zf + &vg + &gs) -> (&rg) -> rx
        // All these need to be high -> for &rg to send low to rx

        // foreach (var module in nextToLast) //-> only &rg
        //     Console.WriteLine((module.Id, module.Type));
        // foreach (var module in nextNextToLast) //-> &kd + &zf + &vg + &gs
        //     Console.WriteLine((module.Id, module.Type));

        int maxCycles = part1 ? 1000 : int.MaxValue;
        int highPulses = 0;
        int lowPulses = 0;

        var queue = new Queue<(string, string, PulseType)>();
        for (int cycles = 1; cycles <= maxCycles; cycles++)
        {
            queue.Enqueue(("broadcaster", "button", PulseType.Low));
            while (queue.Count > 0)
            {
                (string id, string from, PulseType incomingPulseType) = queue.Dequeue();
                //Console.WriteLine($"{from} -{pulseType}-> {id}");

                // Part 1
                if (incomingPulseType == PulseType.Low)
                {
                    lowPulses++;
                }
                else
                {
                    highPulses++;
                }

                // Part 2
                if (incomingPulseType == PulseType.High &&
                    thirdToLast.TryGetValue(from, out int currentCycleValue) &&
                    currentCycleValue == 0)
                {
                    thirdToLast[from] = cycles;
                    if (thirdToLast.All(v => v.Value > 0))
                    {
                        return thirdToLast.Values.LeastCommonMultiple();
                    }
                }

                if (!modules.ContainsKey(id))
                    continue;

                var toQueue = modules[id].HandlePulse(from, incomingPulseType);
                foreach (var item in toQueue)
                {
                    queue.Enqueue(item);
                }
            }
        }
        return highPulses * lowPulses;
    }

    public string PartOne(string input) => PulseNetwork(input).ToString();
    
    public string PartTwo(string input) => PulseNetwork(input, false).ToString();
}