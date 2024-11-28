using MireaConfigurationManagement.Interpreter.Base;

namespace MireaConfigurationManagement.Interpreter.Commands;

public class BitrevCommand : InterpreterCommand
{
    private int B;
    private int C;

    public BitrevCommand(int B, int C)
    {
        this.B = B;
        this.C = C;
    }

    public override void Execute(InterpreterState state)
    {
        int value = state.Memory.ContainsKey(C) ? state.Memory[C] : 0;
        int reversedValue = BitReverse(value);
        state.Registers[B] = reversedValue;
    }

    private int BitReverse(int value)
    {
        uint x = (uint)value;
        x = ((x & 0x55555555) << 1) | ((x >> 1) & 0x55555555);
        x = ((x & 0x33333333) << 2) | ((x >> 2) & 0x33333333);
        x = ((x & 0x0F0F0F0F) << 4) | ((x >> 4) & 0x0F0F0F0F);
        x = ((x & 0x00FF00FF) << 8) | ((x >> 8) & 0x00FF00FF);
        x = (x << 16) | (x >> 16);
        return (int)x;
    }
}