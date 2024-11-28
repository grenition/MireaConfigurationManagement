namespace MireaConfigurationManagement.Interpreter.Base;

public class InterpreterState
{
    public int[] Registers { get; } = new int[8];
    public Dictionary<int, int> Memory { get; } = new Dictionary<int, int>();
}