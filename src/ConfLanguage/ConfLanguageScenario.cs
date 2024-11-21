using MireaConfigurationManagement.Core.Scenarios;
using Newtonsoft.Json;

namespace MireaConfigurationManagement.ConfLanguage;

public class ConfLanguageScenario : IScenario
{
    public string Key => "conf_language";

    public async Task Execute(CancellationToken token)
    {
        try
        {
            Console.WriteLine("Enter JSON data:");

            var lines = new List<string>();
            while (true)
            {
                string? line = Console.ReadLine();
                if (string.IsNullOrEmpty(line))
                    break;
                lines.Add(line);
            }
            var jsonInput = string.Join('\n', lines);

            Console.WriteLine("Enter absolute output file path");
            string outputPath = Console.ReadLine();

            if (string.IsNullOrEmpty(outputPath))
            {
                Console.WriteLine("Invalid file path.");
                return;
            }

            var confConverter = new JsonToConfConverter();
            var confOutputText = confConverter.ConvertFromJson(jsonInput);

            await File.WriteAllTextAsync(outputPath, confOutputText);

            Console.WriteLine($"Converting JSON to Edu Configutaion language completed. Result writed to file {outputPath}");
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"Json parsing error: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}