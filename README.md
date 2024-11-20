# Mirea configaration management homeworks

Для сборки и исполнения программы у вас должна быть установлена среда исполнения .net

Для исполнения программы перейдите в каталог, содержащий файл `MireaConfigurationManagement.csproj`
и введите следующую команду

```
dotnet run MireaConfigurationManagement.csproj
```

# Homework1 - Shell emulator

Команда для перехода к сценарию эмулятора CLI

```
scenario execute shell_emulator
```

Сценарий эмулирует следующие bash команды `ls, cd, pwd, uptime, date`

# Homework 2 - Git dependencies visualizer

Для корректной работы сценария, необходимо, чтобы был установлен [MermaidCLI](https://github.com/mermaid-js/mermaid-cli)

GitDependenciesScenario – это сценарий, предназначенный для анализа и визуализации зависимостей Git-репозитория. Он выполняет слудующие операции

1. Построение графа зависимостей коммитов в репозитории.
2. Сохранение графа в формате Mermaid.
3. Генерация изображения графа в формате PNG с использованием Mermaid CLI.
4. Автоматическое открытие сгенерированного изображения для просмотра.

Команда для перехода к сценарию

```
scenario execute git_dependencies
```
