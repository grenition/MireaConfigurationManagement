using System.Diagnostics;
using MireaConfigurationManagement.Core.Scenarios;
using MireaConfigurationManagement.ShellEmulator.Programms;
using MireaConfigurationManagement.ShellEmulator.Programms.Base;

namespace MireaConfigurationManagement.ShellEmulator;

public class ShellEmulatorScenario : IScenario
{
    public string Key => "shell_emulator";

    private List<IShellProgramm> _shellProgramms = new();

    private void InitializeProgramms()
    {
        _shellProgramms.Add(new HelloProgramm());
    }

    public async Task Execute(CancellationToken token)
    {
        Console.WriteLine("Shell emulator started!");
        
        while (true)
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
                        await programm.Execute(args);
                        break;
                }
            }
            catch (Exception ex)
            {
                if (ex is ParsingCommandException parsingEx)
                {
                    var message = "Parsing commands error";
                    if (!string.IsNullOrEmpty(parsingEx.ParsingMessage))
                        message += $": {parsingEx.ParsingMessage}";
                    Console.WriteLine(message);
                }
                else
                    Console.WriteLine(ex);
            }
        }
    }

    public ShellEmulatorScenario()
    {
        InitializeProgramms();
    }
}
