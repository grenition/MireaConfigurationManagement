namespace MireaConfigurationManagement.Core.Scenarios;

public class ScenariosExecutor
{
    private List<IScenario> _scenarios = new();
    private CancellationTokenSource _cancellationTokenSource;
    private TaskCompletionSource<bool> _exitTcs = new();

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

    public void RestartExecution()
    {
        StopExecution();
        StartExecution();
    }

    private async Task ExecutionPipeline(CancellationToken token)
    {
        Console.WriteLine("Select scenario to execute");
        
        while (!token.IsCancellationRequested)
        {
            try
            {
                var commandLine = Console.ReadLine();
                var commands = commandLine.Split(' ');

                if (commands.Length < 2) throw new ParsingCommandException();

                switch (commands[0])
                {
                    case "exit":
                        _exitTcs.SetResult(true);
                        return;
                    case "scenario":
                        switch (commands[1])
                        {
                            case "execute":
                                if (commands.Length < 3) throw new ParsingCommandException("scenario is not assigned");
                                var scenario = _scenarios.FirstOrDefault(x => x.Key == commands[2]);
                                if(scenario == null) throw new ParsingCommandException($"scenario {commands[2]} is not exists");

                                try
                                {
                                    await scenario.Execute(token);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex);
                                }
                                
                                RestartExecution();
                                break;
                            case "list":
                                PrintRegisteredScenarios();
                                break;
                            default:
                                throw new ParsingCommandException($"unknown scenario command {commands[1]}");
                                break;
                        }
                        break;
                    default:
                        throw new ParsingCommandException($"unknown command {commands[0]}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }

    private void PrintRegisteredScenarios()
    {
        Console.WriteLine("Registered scenarios: ");
        foreach (var scenario in _scenarios)
            Console.WriteLine($"\t {scenario.Key}");
    }

    public async Task ExitRequest()
    {
        await _exitTcs.Task;
    }
    
    #endregion
}

public class ParsingCommandException : Exception
{
    public ParsingCommandException() : base(string.Empty) { }
    public ParsingCommandException(string parsingMessage) : base(parsingMessage) { }
    public override string ToString()
    {
        var message = "Parsing commands error";
        if (!string.IsNullOrEmpty(Message))
            message += $": {Message}";
        return message;
    }
}