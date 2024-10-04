using MireaConfigurationManagement.Core.Extensions;
using MireaConfigurationManagement.ShellEmulator.Programms.Base;
using MireaConfigurationManagement.ShellEmulator.System;

namespace MireaConfigurationManagement.ShellEmulator.Programms;

public class LsProgramm : IShellProgramm
{
    public string Key => "ls";
    public async Task Execute(IEnumerable<string> args, ShellSystem system)
    {
        string GetEntry(string raw)
        {
            if (raw.StartsWith(system.PathPointer) && raw.Length >= system.PathPointer.Length)
                return raw.Substring(system.PathPointer.Length, raw.Length - system.PathPointer.Length);
            return raw;
        }

        void PrintEntry(string entry, bool newLine = true)
        {
            if(entry.Length == 0) return;
            
            if (newLine)
                Console.WriteLine(entry);
            else
                Console.Write(entry);
        }


        var entries = system.Entries
            .Where(x => x.StartsWith(system.PathPointer));

        if (args.Count() > 0 && args.First() == "-1")
        {
            foreach (var entry in entries)
            {
                PrintEntry(GetEntry(entry), true);
            }
        }
        else
        {
            foreach (var entry in entries)
            {
                PrintEntry(GetEntry(entry) + "\t", false);
            }
            Console.Write("\n");
        }
    }
}
