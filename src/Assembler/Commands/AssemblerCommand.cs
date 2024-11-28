namespace MireaConfigurationManagement.Assembler;

public abstract class AssemblerCommand
{
    public abstract string Name { get; }
    
    public abstract bool TryParse(string[] tokens);

    public abstract byte[] Assemble();
    
    public abstract string GetLogEntry();
}
