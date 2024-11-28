using MireaConfigurationManagement.Interpreter.Base;

namespace MireaConfigurationManagement.Interpreter.Commands;

public abstract class InterpreterCommand
{
    public abstract void Execute(InterpreterState state);
}
