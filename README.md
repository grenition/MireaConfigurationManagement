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

# Homework 3 - Edu configuration language

Инструмент командной строки для учебного конфигурационного. Этот инструмент преобразует текст из
входного формата в выходной. Синтаксические ошибки выявляются с выдачей
сообщений.
Входной текст на языке json принимается из стандартного ввода. Выходной
текст на учебном конфигурационном языке попадает в файл, путь к которому
задан ключом командной строки.

Команда для перехода к сценарию

```
scenario execute conf_language
```

Пример входных данных
```
{
  /* Это комментарий к корневому объекту */
  "configName": "TestConfig", // Комментарий к полю configName
  "version": 1,
  "settings": {
    // Комментарий внутри settings
    "maxUsers": 100,
    "features": [
      "login",
      "dashboard",
      "reports"
    ]
  }
}
```
Выходной файл
```
table(
    {#
    Это комментарий к корневому объекту
    #}
    configName => 'TestConfig',
    {#
    Комментарий к полю configName
    #}
    version => 1,
    settings => table(
        {#
        Комментарий внутри settings
        #}
        maxUsers => 100,
        features => ['login'; 'dashboard'; 'reports']
    )
)
```