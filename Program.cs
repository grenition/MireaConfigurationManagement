using MireaConfigurationManagement.Core.Scenarios;
using MireaConfigurationManagement.ShellEmulator;

var executor = new ScanariosExecutor();

executor.RegisterScenario(new ShellEmulatorScenario());

executor.StartExecution();