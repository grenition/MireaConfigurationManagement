using MireaConfigurationManagement.ShellEmulator.Programms.Base;
using MireaConfigurationManagement.ShellEmulator.System;

namespace MireaConfigurationManagement.ShellEmulator.Programms;

public class CdProgramm : IShellProgramm
{
    public string Key => "cd";
    public async Task Execute(IEnumerable<string> args, ShellSystem system)
    {
        if(args.Count() == 0) return;

        system.PathPointer = args.First();

        if (!system.PathPointer.StartsWith('/'))
            system.PathPointer = '/' + system.PathPointer;
    }
}
