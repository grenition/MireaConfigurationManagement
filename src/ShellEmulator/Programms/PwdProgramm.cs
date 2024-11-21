using MireaConfigurationManagement.Core.Extensions;
using MireaConfigurationManagement.ShellEmulator.Programms.Base;
using MireaConfigurationManagement.ShellEmulator.System;

namespace MireaConfigurationManagement.ShellEmulator.Programms;

public class PwdProgramm : IShellProgramm
{
    public string Key => "pwd";
    public async Task Execute(IEnumerable<string> args, ShellSystem system)
    {
        Console.WriteLine(system.PathPointer);
    }
}
