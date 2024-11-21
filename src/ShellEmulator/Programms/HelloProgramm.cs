using MireaConfigurationManagement.ShellEmulator.Programms.Base;
using MireaConfigurationManagement.ShellEmulator.System;

namespace MireaConfigurationManagement.ShellEmulator.Programms;

public class HelloProgramm : IShellProgramm
{
    public string Key => "hello";
    
    public async Task Execute(IEnumerable<string> args, ShellSystem system)
    {
        Console.WriteLine("Hello world!");
    }
}
