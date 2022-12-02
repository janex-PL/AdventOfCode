using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.ConsoleApp._2015;

public class Day07
{
    public static void Execute()
    {
        var data = DataProvider.GetData(2015, 7).Split("\r\n");
        Console.WriteLine(GetSignalFromWireA(data));
    }

    private static uint GetSignalFromWireA(string[] circuitData)
    {
        var circuit = new Circuit();

        circuit.AddAllWires(circuitData);
                
        circuit.AddAllConstantSources(circuitData);

        circuit.ConnectWiresToWires(circuitData);

        circuit.ConnectConstantSourcesToWires(circuitData);

        circuit.AddNotBlocks(circuitData);

        circuit.AddOtherBlocks(circuitData);

        var wireB = circuit.Wires.First(x => x.Name == "b");

        var aValue = circuit.Wires.First(x => x.Name == "a").GetValue();
        wireB.Value = aValue;

        for (int i = 0; i < circuit.Wires.Count; i++)
        {
            if (circuit.Wires[i].Name != "b")
                circuit.Wires[i].Value = null;
        }

        for (int i = 0; i < circuit.Blocks.Count; i++)
        {
            circuit.Blocks[i].Value = null;
        }

        aValue = circuit.Wires.First(x => x.Name == "a").GetValue();
        return 1;

    }

        

}

public class Circuit
{
    public List<Wire> Wires { get; set; } = new();
    public List<Block> Blocks { get; set; } = new();
    public List<ConstantSource> ConstantSources { get; set; } = new();

    public void ConnectConstantSourcesToWires(IEnumerable<string> circuitData)
    {
        var connections = circuitData.Where(x => Regex.IsMatch(x, @"^\d+ -> [a-z]+$")).Select(x => x.Split(" -> "))
            .ToList();

        foreach (var connection in connections)
        {
            var source = ConstantSources.First(x => x.Value == uint.Parse(connection.First()));
            var wire = Wires.First(x => x.Name == connection.Last());

            source.Outputs.Add(wire);
            wire.Source = source;
        }
    }

    public void AddAllWires(IEnumerable<string> circuitData)
    {
        var wireNames = Regex.Matches(string.Join(' ', circuitData), @"[a-z]+").Select(x => x.Value).Distinct();

        foreach (var wireName in wireNames)
        {
            if (Wires.Any(x => x.Name == wireName)) 
                continue;

            var wire = new Wire()
            {
                Name = wireName
            };

            Wires.Add(wire);
        }
    }

    public void AddAllConstantSources(IEnumerable<string> circuitData)
    {
        var constantSourceValues = Regex.Matches(string.Join(' ', circuitData), @"\d+").Select(x => uint.Parse(x.Value)).Distinct();

        foreach (var constantSourceValue in constantSourceValues)
        {
            if (ConstantSources.Any(x => x.Value == constantSourceValue)) 
                continue;

            var constantSource = new ConstantSource()
            {
                Value = constantSourceValue
            };

            ConstantSources.Add(constantSource);
        }
    }

    public void ConnectWiresToWires(IEnumerable<string> circuitData)
    {
        var connections = circuitData.Where(x => Regex.IsMatch(x, @"^[a-z]+ -> [a-z]+$")).Select(x => x.Split(" -> "))
            .ToList();

        foreach (var connection in connections)
        {
            var firstWire = Wires.First(x => x.Name == connection.First());
            var lastWire = Wires.First(x => x.Name == connection.Last());

            firstWire.Outputs.Add(lastWire);
            lastWire.Source = firstWire;
        }

    }

    public void AddNotBlocks(IEnumerable<string> circuitData)
    {
        var blocksData = circuitData.Where(x => x.Contains("NOT")).Select(x => x.Split(' ').Where(y => y != "->").ToArray()).ToArray();

        foreach (var blockData in blocksData)
        {
            var inputWire = Wires.First(x => x.Name == blockData[1]);
            var outputWire = Wires.First(x => x.Name == blockData[2]);

            var block = new Block()
            {
                Operation = BitwiseOperation.NOT,
                FirstSource = inputWire,
                Output = outputWire
            };
            inputWire.Outputs.Add(block);
            outputWire.Source = block;

            Blocks.Add(block);
        }
    }

    public void AddOtherBlocks(IEnumerable<string> circuitData)
    {
        var blocksData = circuitData.Where(x => Regex.IsMatch(x, "[A-Z]+") && !x.Contains("NOT"))
            .Select(x => x.Split(' ').Where(y => y != "->").ToArray()).ToArray();

        foreach (var blockData in blocksData)
        {
            var block = new Block()
            {
                Operation = Enum.Parse<BitwiseOperation>(blockData[1])
            };
            if (Regex.IsMatch(blockData[0], @"\d+"))
            {
                var first = ConstantSources.First(x => x.Value == uint.Parse(blockData[0]));
                block.FirstSource = first;
                first.Outputs.Add(block);
            }
            else
            {
                var first = Wires.First(x => x.Name == blockData[0]);
                block.FirstSource = first;
                first.Outputs.Add(block);
            }

            if (Regex.IsMatch(blockData[2], @"\d+"))
            {
                var second = ConstantSources.First(x => x.Value == uint.Parse(blockData[2]));
                block.SecondSource = second;
                second.Outputs.Add(block);
            }
            else
            {
                var second = Wires.First(x => x.Name == blockData[2]);
                block.SecondSource = second;
                second.Outputs.Add(block);
            }
            var output = Wires.First(x => x.Name == blockData.Last());

            block.Output = output;
            output.Source = block;

            Blocks.Add(block);
        }
    }
}



public class Wire : ICircuitComponent, ISingleSourced, IMultiOutput
{
    public string Name { get; set; } = string.Empty;
    public uint? Value { get; set; }

    public uint GetValue()
    {
        Value ??= Source?.GetValue();

        return Value ?? throw new NullReferenceException();
    }

    public ICircuitComponent Source { get; set; }
    public List<ICircuitComponent> Outputs { get; set; } = new List<ICircuitComponent>();
}
public class ConstantSource : ICircuitComponent, IMultiOutput
{
    public uint? Value { get; set; }
    public uint GetValue()
    {
        return Value ?? throw new NullReferenceException();
    }

    public List<ICircuitComponent> Outputs { get; set; } = new List<ICircuitComponent>();
}
public class Block : ICircuitComponent, IDoubleSourced, ISingleOutput
{
    public uint? Value { get; set; }
    public uint GetValue()
    {
        if (Value == null)
            CalculateValue();

        return Value ?? throw new NullReferenceException();
    }

    private void CalculateValue()
    {
        if (Value.HasValue)
            return;
        if (Operation == BitwiseOperation.None)
            throw new InvalidOperationException();

        var firstInput = FirstSource?.GetValue() ?? throw new NullReferenceException();
        uint secondInput = 0;

        if(Operation != BitwiseOperation.NOT)
            secondInput = SecondSource?.GetValue() ?? throw new NullReferenceException();

        Value = Operation switch
        {
            BitwiseOperation.NOT => ~firstInput,
            BitwiseOperation.AND => firstInput & secondInput,
            BitwiseOperation.OR => firstInput | secondInput,
            BitwiseOperation.LSHIFT => firstInput << (int)secondInput,
            BitwiseOperation.RSHIFT => firstInput >> (int)secondInput,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public ICircuitComponent FirstSource { get; set; }
    public ICircuitComponent SecondSource { get; set; }
    public ICircuitComponent Output { get; set; }
    public BitwiseOperation Operation { get; set; } = BitwiseOperation.None;
}

public interface ICircuitComponent
{
    public uint? Value { get; set; }
    public uint GetValue();
}

public interface ISingleSourced
{
    public ICircuitComponent Source { get; set; }
}
public interface IDoubleSourced
{
    public ICircuitComponent FirstSource { get; set; }
    public ICircuitComponent SecondSource { get; set; }

}
public interface ISingleOutput
{
    public ICircuitComponent Output { get; set; }
}

public interface IMultiOutput
{
    public List<ICircuitComponent> Outputs { get; set; }
}

public enum BitwiseOperation
{
    None = -1,
    AND,
    OR,
    LSHIFT,
    RSHIFT,
    NOT
}