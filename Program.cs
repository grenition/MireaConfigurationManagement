using MireaConfigurationManagement.Core.Scenarios;
using MireaConfigurationManagement.ShellEmulator;

var executor = new ScenariosExecutor();

executor.RegisterScenario(new ShellEmulatorScenario());

executor.StartExecution();

await executor.ExitRequest();