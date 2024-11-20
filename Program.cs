using MireaConfigurationManagement.Core.Scenarios;
using MireaConfigurationManagement.GitDependencies;
using MireaConfigurationManagement.ShellEmulator;

var executor = new ScenariosExecutor();

executor.RegisterScenario(new ShellEmulatorScenario());
executor.RegisterScenario(new GitDependenciesScenario());

executor.StartExecution();

await executor.ExitRequest();