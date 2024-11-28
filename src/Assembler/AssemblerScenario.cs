using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MireaConfigurationManagement.Core.Scenarios;

namespace MireaConfigurationManagement.Assembler
{
    public class AssemblerScenario : IScenario
    {
        public string Key => "assembler";

        public async Task Execute(CancellationToken token)
        {
            Console.WriteLine("Enter source file path");
            string inputFile = Console.ReadLine();
            Console.WriteLine("Enter binary file path");
            string outputFile = Console.ReadLine();
            Console.WriteLine("Enter log file path");
            string logFile = Console.ReadLine();
            
            using (BinaryWriter writer = new BinaryWriter(File.Open(outputFile, FileMode.Create)))
            using (StreamWriter logfile = new StreamWriter(logFile))
            {
                using (StreamReader infile = new StreamReader(inputFile))
                {
                    while (!infile.EndOfStream)
                    {
                        var line = await infile.ReadLineAsync();
                        
                        if (token.IsCancellationRequested)
                            token.ThrowIfCancellationRequested();

                        line = line.Trim();

                        if (string.IsNullOrEmpty(line) || line.StartsWith("#"))
                            continue;

                        string[] tokens = line.Split(new char[]
                        {
                            ' ', '\t', ','
                        }, StringSplitOptions.RemoveEmptyEntries);

                        if (tokens.Length == 0)
                            continue;

                        AssemblerCommand command = null;

                        List<AssemblerCommand> possibleCommands = new List<AssemblerCommand>
                        {
                            new LoadConstCommand(),
                            new ReadMemCommand(),
                            new WriteMemCommand(),
                            new BitrevCommand()
                        };

                        bool parsed = false;
                        foreach (var cmd in possibleCommands)
                        {
                            try
                            {
                                if (cmd.TryParse(tokens))
                                {
                                    command = cmd;
                                    parsed = true;
                                    break;
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.Error.WriteLine($"Error parsing instruction '{line}': {ex.Message}");
                                parsed = true;
                                break;
                            }
                        }

                        if (!parsed)
                        {
                            Console.Error.WriteLine($"Unknown instruction: '{line}'");
                            continue;
                        }

                        byte[] code = command.Assemble();
                        writer.Write(code);

                        logfile.WriteLine(command.GetLogEntry());
                    }
                }

                Console.WriteLine("Assembly completed successfully.");
            }

        }
    }
}
