﻿using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace AoC20
{
    public class Day08
    {
        private const string Exmaple =
            @"nop +0
acc +1
jmp +4
acc +3
jmp -3
acc -99
acc +1
jmp -4
acc +6";

        [Fact]
        public void Can_parse_Noop()
        {
            new BootCode(Exmaple).Instructions.First()
                .Should().BeOfType(typeof(Noop));
        }

        [Fact]
        public void Can_parse_Accumulate()
        {
            new BootCode(Exmaple).Instructions.Skip(1).Take(1)
                .Should().BeEquivalentTo(new Accumulate(1));
        }

        [Fact]
        public void Can_parse_Jump()
        {
            new BootCode(Exmaple).Instructions.Skip(2).Take(1)
                .Should().BeEquivalentTo(new Jump(4));
        }

        [Fact]
        public void Executing_Noop_adds_it_to_executed()
        {
            new BootCode(Exmaple).Execute(1).ExecutedInstructions.FirstOrDefault()
                .Should().BeOfType(typeof(Noop));
        }
        
        [Fact]
        public void NextInstruction_is_the_first_one_by_default()
        {
            var bootCode = new BootCode(Exmaple);

            bootCode.NextInstruction
                .Should().Be(bootCode.Instructions.First());
        }
        
        [Fact]
        public void Executing_Noop_moves_next_instruction_forward()
        {
            var bootCode = new BootCode(Exmaple);

            bootCode.Execute(1).NextInstruction
                .Should().Be(bootCode.Instructions.Skip(1).First());
        }
        
        [Fact]
        public void Accumulator_is_zero_by_default()
        {
            new BootCode(Exmaple).Accumulator
                .Should().Be(0);
        }
        
        // [Fact]
        // public void Executing_Accumulate_changes_accumulator_by_its_value()
        // {
        //     var bootCode = new BootCode(Exmaple);
        //
        //     bootCode.Execute(1).NextInstruction
        //         .Should().Be(bootCode.Instructions.Skip(1).First());
        // }
    }

    public interface IInstruction
    {
    }

    public class Noop : IInstruction
    {
    }

    public class Accumulate : IInstruction
    {
        public Accumulate(int change)
        {
            Change = change;
        }

        public int Change { get; }
    }

    public class Jump : IInstruction
    {
        public Jump(int offset)
        {
            Offset = offset;
        }

        public int Offset { get; }
    }

    public class BootCode
    {
        public BootCode(string raw)
        {
            Instructions = ParseInstructions(raw);
            ExecutedInstructions = Enumerable.Empty<IInstruction>();
            NextInstruction = Instructions.First();
        }

        private BootCode(BootCode old)
        {
            Instructions = old.Instructions;
            ExecutedInstructions = old.ExecutedInstructions;
        }

        public IEnumerable<IInstruction> Instructions { get; }
        
        public IEnumerable<IInstruction> ExecutedInstructions { get; private set; }

        public IInstruction NextInstruction { get; private set; }

        public int Accumulator { get; }

        private static IInstruction[] ParseInstructions(string raw)
        {
            return raw.Split(Environment.NewLine)
                .Select(line => line.Split(' '))
                .Select(ParseInstruction)
                .ToArray();
        }

        private static IInstruction ParseInstruction(string[] tokens)
        {
            return tokens[0] switch
            {
                "nop" => new Noop(),
                "jmp" => new Jump(int.Parse(tokens[1])),
                "acc" => new Accumulate(int.Parse(tokens[1])),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public BootCode Execute(int numberOfInstructions)
        {
            return
                new BootCode(this)
                {
                    ExecutedInstructions = ExecutedInstructions.Append(Instructions.First()),
                    NextInstruction = Instructions.Skip(1).First(), 
                };
        }
    }
}