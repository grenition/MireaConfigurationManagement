# Mirea configaration management homeworks

Для сборки и исполнения программы у вас должна быть установлена среда исполнения .net

```
здесь должны быть команды на установку dotnet для разных os
```

Для исполнения программы перейдите в каталог, содержащий файл ```MireaConfigurationManagement.csproj```
и введите следующую команду

```
dotnet run MireaConfigurationManagement.csproj
```

# Homework1 - Shell emulator

Команда для перехода к сценарию эмулятора CLI

```
scenario execute shell_emulator
```

Сценарий эмулирует следующие bash команды ```ls, cd, pwd, uptime, date```

# Homework 2 - Git dependencies visualizer

Для корректной работы сценария, необходимо, чтобы был установлен MermaidCLI

Команда для установки (через npm, поэтому необходим установленный node.js):
```
npm install -g @mermaid-js/mermaid-cli
```