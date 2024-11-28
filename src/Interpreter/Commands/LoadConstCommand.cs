using MireaConfigurationManagement.Interpreter.Base;

namespace MireaConfigurationManagement.Interpreter.Commands;

public class LoadConstCommand : InterpreterCommand
{
    private int B;
    private int C;

    public LoadConstCommand(int B, int C)
    {
        this.B = B;
        this.C = C;
    }

    public override void Execute(InterpreterState state)
    {
        state.Registers[C] = B;
    }
}