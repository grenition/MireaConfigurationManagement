using MireaConfigurationManagement.Interpreter.Commands;

namespace MireaConfigurationManagement.Interpreter.Base;

using System;
using System.Collections.Generic;
using System.IO;

public class Interpreter
{
    private InterpreterState state = new InterpreterState();
    private List<InterpreterCommand> program = new List<InterpreterCommand>();

    public void LoadProgram(byte[] binaryCode)
    {
        int pc = 0;
        while (pc < binaryCode.Length)
        {
            InterpreterCommand command = ParseInstruction(binaryCode, ref pc);
            program.Add(command);
        }
    }

    private InterpreterCommand ParseInstruction(byte[] code, ref int pc)
    {
        if (pc >= code.Length)
            throw new Exception("Unhandled code ending");

        byte firstByte = code[pc];
        int A = firstByte & 0x3F;

        switch (A)
        {
            case 28: // LOAD_CONST
                {
                    if (pc + 3 > code.Length)
                        throw new Exception("Incorrect lenght of instruction LOAD_CONST");

                    uint instructionValue = (uint)(code[pc] | (code[pc + 1] << 8) | (code[pc + 2] << 16));
                    int B = (int)((instructionValue >> 6) & 0x3FFF);
                    int C = (int)((instructionValue >> 20) & 0x7);  

                    pc += 3;

                    return new LoadConstCommand(B, C);
                }

            case 62: // READ_MEM
                {
                    if (pc + 3 > code.Length)
                        throw new Exception("Incorrect lenght of instruction READ_MEM");

                    uint instructionValue = (uint)(code[pc] | (code[pc + 1] << 8) | (code[pc + 2] << 16));
                    int B = (int)((instructionValue >> 6) & 0x7);   
                    int C = (int)((instructionValue >> 9) & 0x7);   
                    int D = (int)((instructionValue >> 12) & 0x7FF);

                    pc += 3;

                    return new ReadMemCommand(B, C, D);
                }

            case 3: // WRITE_MEM
                {
                    if (pc + 3 > code.Length)
                        throw new Exception("Incorrect lenght of instruction WRITE_MEM");

                    uint instructionValue = (uint)(code[pc] | (code[pc + 1] << 8) | (code[pc + 2] << 16));
                    int B = (int)((instructionValue >> 6) & 0x7);   
                    int C = (int)((instructionValue >> 9) & 0x7);   
                    int D = (int)((instructionValue >> 12) & 0x7FF);

                    pc += 3;

                    return new WriteMemCommand(B, C, D);
                }

            case 58: // BITREV
                {
                    if (pc + 4 > code.Length)
                        throw new Exception("Incorrect lenght of instruction BITREV");

                    uint instructionValue = (uint)(code[pc] | (code[pc + 1] << 8) | (code[pc + 2] << 16) | (code[pc + 3] << 24));
                    int B = (int)((instructionValue >> 6) & 0x7);      
                    int C = (int)((instructionValue >> 9) & 0x3FFFFF); 

                    pc += 4;

                    return new BitrevCommand(B, C);
                }

            default:
                throw new Exception($"Unknown operation code: {A}");
        }
    }

    public void ExecuteProgram()
    {
        foreach (var command in program)
        {
            command.Execute(state);
        }
    }

    public void DumpMemory(string filePath, int startAddress, int endAddress)
    {
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            writer.WriteLine("Address,Value");
            for (int address = startAddress; address <= endAddress; address++)
            {
                int value = state.Memory.ContainsKey(address) ? state.Memory[address] : 0;
                writer.WriteLine($"{address},{value}");
            }
        }
    }
}
