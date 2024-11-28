using MireaConfigurationManagement.Assembler;
using MireaConfigurationManagement.ConfLanguage;
using MireaConfigurationManagement.Core.Scenarios;
using MireaConfigurationManagement.GitDependencies;
using MireaConfigurationManagement.Interpreter;
using MireaConfigurationManagement.ShellEmulator;

var executor = new ScenariosExecutor();

executor.RegisterScenario(new ShellEmulatorScenario());
executor.RegisterScenario(new GitDependenciesScenario());
executor.RegisterScenario(new ConfLanguageScenario());
executor.RegisterScenario(new AssemblerScenario());
executor.RegisterScenario(new InterpreterScenario());

executor.StartExecution();

await executor.ExitRequest();