using MireaConfigurationManagement.ShellEmulator.Programms.Base;
using MireaConfigurationManagement.ShellEmulator.System;

namespace MireaConfigurationManagement.ShellEmulator.Programms;

public class DateProgramm : IShellProgramm
{
    public string Key => "date";
    public async Task Execute(IEnumerable<string> args, ShellSystem system)
    {
        Console.WriteLine(DateTime.Now.ToString("ddd MMM d HH:mm:ss yyyy"));
    }
}
