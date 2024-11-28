using MireaConfigurationManagement.Core.Scenarios;

namespace MireaConfigurationManagement.Interpreter;

public class InterpreterScenario : IScenario
{
    public string Key => "interpreter";
    
    public async Task Execute(CancellationToken token)
    {
        Console.WriteLine("Enter binary file path");
        string binaryFilePath = Console.ReadLine();
        Console.WriteLine("Enter result file path");
        string resultFilePath = Console.ReadLine();
        Console.WriteLine("Enter memory range inf format startAddress-endAdress");
        string memoryRange = Console.ReadLine();

        string[] rangeParts = memoryRange.Split('-');
        if (rangeParts.Length != 2 ||
            !int.TryParse(rangeParts[0], out int memoryStart) ||
            !int.TryParse(rangeParts[1], out int memoryEnd))
        {
            Console.WriteLine("Incorrect memory range format");
            return;
        }

        if (memoryStart < 0 || memoryEnd < memoryStart)
        {
            Console.WriteLine("Incorrect memory range diapazone");
            return;
        }

        byte[] binaryCode = await File.ReadAllBytesAsync(binaryFilePath, token);

        Base.Interpreter interpreter = new Base.Interpreter();

        interpreter.LoadProgram(binaryCode);
        interpreter.ExecuteProgram();

        interpreter.DumpMemory(resultFilePath, memoryStart, memoryEnd);

        Console.WriteLine($"Interprieting completed succesfully. Results writed at {resultFilePath}");
    }
}
