using MireaConfigurationManagement.Interpreter.Base;

namespace MireaConfigurationManagement.Interpreter.Commands;

public class ReadMemCommand : InterpreterCommand
{
    private int B;
    private int C;
    private int D;

    public ReadMemCommand(int B, int C, int D)
    {
        this.B = B;
        this.C = C;
        this.D = D;
    }

    public override void Execute(InterpreterState state)
    {
        int baseAddress = state.Registers[B];
        int memoryAddress = baseAddress + D;
        int value = state.Memory.ContainsKey(memoryAddress) ? state.Memory[memoryAddress] : 0;
        state.Registers[C] = value;
    }
}
