namespace MireaConfigurationManagement.Core.Scenarios;

public interface IScenario
{
    public string Key { get; }
    public Task Execute(CancellationToken token);
}
