using MireaConfigurationManagement.Core.Scenarios;
using MireaConfigurationManagement.Homework1;

var executor = new ScanariosExecutor();

executor.RegisterScenario(new FirstHomeworkScenario());

executor.StartExecution();