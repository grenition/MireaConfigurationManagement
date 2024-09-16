using System.Diagnostics;
using MireaConfigurationManagement.Core.Scenarios;

namespace MireaConfigurationManagement.Homework1;

public class FirstHomeworkScenario : IScenario
{
    public string Key => nameof(FirstHomeworkScenario);
    
    public async Task Execute(CancellationToken token)
    {
        Console.WriteLine("Homework task completed");
    }
}
