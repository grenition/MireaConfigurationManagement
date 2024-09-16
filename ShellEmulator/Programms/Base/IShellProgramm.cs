namespace MireaConfigurationManagement.ShellEmulator.Programms.Base;

public interface IShellProgramm
{
    public string Key { get; }
    public Task Execute(IEnumerable<string> args);
}
