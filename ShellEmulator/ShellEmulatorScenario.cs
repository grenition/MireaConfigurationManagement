using System.Diagnostics;
using MireaConfigurationManagement.Core.Scenarios;
using MireaConfigurationManagement.ShellEmulator.Programms;
using MireaConfigurationManagement.ShellEmulator.Programms.Base;
using MireaConfigurationManagement.ShellEmulator.System;

namespace MireaConfigurationManagement.ShellEmulator;

public class ShellEmulatorScenario : IScenario
{
    public string Key => "shell_emulator";

    private List<IShellProgramm> _shellProgramms;
    private ShellSystem _shellSystem;

    private void Initialize()
    {
        _shellSystem = new();

        _shellProgramms = new();
        _shellProgramms.Add(new HelloProgramm());
        _shellProgramms.Add(new LsProgramm());
        _shellProgramms.Add(new CdProgramm());
        _shellProgramms.Add(new PwdProgramm());
    }

    public async Task Execute(CancellationToken token)
    {
        Initialize();
        Console.WriteLine("Shell emulator started!");
        
        while (!token.IsCancellationRequested)
        {
            try
            {
                var commandLine = Console.ReadLine();
                var commands = commandLine.Split(' ');

                if (commands.Length < 1) throw new ParsingCommandException();

                switch (commands[0])
                {
                    case "exit":
                        return;
                    default:
                        var programm = _shellProgramms.FirstOrDefault(x => x.Key == commands[0]);
                        if(programm == null) throw new ParsingCommandException("programm is not exists");
                        var args = commands.Skip(1);
                        await programm.Execute(args, _shellSystem);
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
