namespace MireaConfigurationManagement.Assembler;

public abstract class AssemblerCommand
{
    public abstract string Name { get; }
    
    public abstract bool TryParse(string[] tokens, out AssemblerCommand command);

    public abstract byte[] Assemble();
    
    public abstract string GetLogEntry();
}
