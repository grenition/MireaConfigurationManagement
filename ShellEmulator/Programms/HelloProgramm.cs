using MireaConfigurationManagement.ShellEmulator.Programms.Base;

namespace MireaConfigurationManagement.ShellEmulator.Programms;

public class HelloProgramm : IShellProgramm
{
    public string Key => "hello";
    
    public async Task Execute(IEnumerable<string> args)
    {
        Console.WriteLine("Asalam aleykum");
    }
}
