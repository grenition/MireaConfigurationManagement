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
            string sourceFilePath = Console.ReadLine();
            Console.WriteLine("Enter binary file path");
            string binaryFilePath = Console.ReadLine();
            Console.WriteLine("Enter log file path");
            string logFilePath = Console.ReadLine();

            string[] sourceLines = await File.ReadAllLinesAsync(sourceFilePath, token);

            List<byte> binaryInstructions = new List<byte>();
            List<string> logEntries = new List<string>();

            List<AssemblerCommand> commandParsers = new List<AssemblerCommand>
            {
                new LoadConstCommand(),
                new ReadMemCommand(),
                new WriteMemCommand(),
                new BitrevCommand()
            };

            foreach (string line in sourceLines)
            {
                string trimmedLine = line.Trim();

                if (string.IsNullOrEmpty(trimmedLine) || trimmedLine.StartsWith("#"))
                {
                    continue;
                }

                string[] tokens = trimmedLine.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                if (tokens.Length == 0)
                {
                    continue;
                }

                AssemblerCommand command = null;
                foreach (var parser in commandParsers)
                {
                    if (parser.TryParse(tokens, out command))
                    {
                        break;
                    }
                }

                if (command == null)
                {
                    Console.WriteLine($"Undefined instruction: {line}");
                    continue;
                }

                byte[] instructionBytes = command.Assemble();
                binaryInstructions.AddRange(instructionBytes);

                logEntries.Add(command.GetLogEntry());
            }

            await File.WriteAllBytesAsync(binaryFilePath, binaryInstructions.ToArray(), token);

            await File.WriteAllLinesAsync(logFilePath, logEntries, token);
        }
    }
}