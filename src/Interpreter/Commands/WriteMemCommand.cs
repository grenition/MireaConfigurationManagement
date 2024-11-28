using MireaConfigurationManagement.Interpreter.Base;

namespace MireaConfigurationManagement.Interpreter.Commands;

public class WriteMemCommand : InterpreterCommand
{
    private int B;
    private int C;
    private int D;

    public WriteMemCommand(int B, int C, int D)
    {
        this.B = B;
        this.C = C;
        this.D = D;
    }

    public override void Execute(InterpreterState state)
    {
        int value = state.Registers[B];
        int baseAddress = state.Registers[C];
        int memoryAddress = baseAddress + D;
        state.Memory[memoryAddress] = value;
    }
}
