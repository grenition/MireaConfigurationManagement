namespace MireaConfigurationManagement.Core.Scenarios;

public class ScanariosExecutor
{
    private List<IScenario> _scenarios = new();
    private CancellationTokenSource _cancellationTokenSource;

    #region Registering
    
    public void RegisterScenario(IScenario scenario)
    {
        if (scenario != null && !_scenarios.Contains(scenario))
            _scenarios.Add(scenario);
    }
    public void UnregisterScenario(IScenario scenario)
    {
        if (_scenarios.Contains(scenario))
            _scenarios.Remove(scenario);
    }
    
    #endregion

    #region Execution

    public void StartExecution()
    {
        StopExecution();
        _cancellationTokenSource = new();
        
        ExecutionPipeline(_cancellationTokenSource.Token);
    }

    public void StopExecution()
    {
        if(_cancellationTokenSource == null) return;

        _cancellationTokenSource.Cancel();
        _cancellationTokenSource.Dispose();
        _cancellationTokenSource = null;
    }

    private async Task ExecutionPipeline(CancellationToken token)
    {
        while (true)
        {
            try
            {
                var commandLine = Console.ReadLine();
                var commands = commandLine.Split(' ');

                if (commands.Length < 2) throw new ParsingCommandExeption();

                switch (commands[0])
                {
                    case "exit":
                        return;
                    case "scenario":
                        switch (commands[1])
                        {
                            case "execute":
                                if (commands.Length < 3) throw new ParsingCommandExeption();
                                var scenario = _scenarios.FirstOrDefault(x => x.Key == commands[2]);
                                if(scenario == null) throw new ParsingCommandExeption();

                                scenario.Execute(token);
                                break;
                            case "list":
                                PrintRegisteredScenarios();
                                break;
                        }
                        break;
                    default:
                        throw new ParsingCommandExeption();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex is ParsingCommandExeption ? "Parsing command error" : ex);
            }
        }
    }

    private void PrintRegisteredScenarios()
    {
        Console.WriteLine("Registered scenarios: ");
        foreach (var scenario in _scenarios)
            Console.WriteLine($"\t {scenario.Key}");
    }

    #endregion
}

public class ParsingCommandExeption : Exception { }