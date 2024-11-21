using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using MireaConfigurationManagement.Core.Scenarios;

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

            using JsonDocument document = JsonDocument.Parse(jsonInput);
            var rootElement = document.RootElement;

            var confLanguageOutput = ConvertJsonToConfLanguage(rootElement);

            await File.WriteAllTextAsync(outputPath, confLanguageOutput);

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

    private string ConvertJsonToConfLanguage(JsonElement element, int nestingLevel = 0)
    {
        string GetNesting()
        {
            var nesting = string.Empty;
            for (int i = 0; i < nestingLevel; i++)
                nesting += '\t';
            return nesting;
        }
        
        switch (element.ValueKind)
        {
            case JsonValueKind.Object:
                return ConvertObject(element, nestingLevel);
            case JsonValueKind.Array:
                return ConvertArray(element, nestingLevel);
            case JsonValueKind.String:
                return GetNesting() + $"'{EscapeString(element.GetString())}'";
            case JsonValueKind.Number:
                return GetNesting() + element.GetRawText();
            case JsonValueKind.True:
            case JsonValueKind.False:
                return GetNesting() + element.GetRawText();
            case JsonValueKind.Null:
                return GetNesting() + "null";
            default:
                throw new Exception("Unsupported JSON value kind.");
        }
    }

    private string ConvertObject(JsonElement element, int nestingLevel = 0)
    {
        var sb = new StringBuilder();
        sb.AppendLine("table(");

        bool first = true;
        foreach (var property in element.EnumerateObject())
        {
            if (!first)
                sb.AppendLine(",");
            first = false;

            var name = property.Name;
            if (!IsValidName(name))
                throw new Exception($"Invalid name: {name}");

            var value = ConvertJsonToConfLanguage(property.Value, nestingLevel + 1);
            sb.Append($" {name} => {value}");
        }

        sb.AppendLine();
        sb.Append(")");
        return sb.ToString();
    }

    private string ConvertArray(JsonElement element, int nestingLevel = 0)
    {
        var sb = new StringBuilder();
        sb.Append("[ ");

        bool first = true;
        foreach (var item in element.EnumerateArray())
        {
            if (!first)
                sb.Append("; ");
            first = false;

            var value = ConvertJsonToConfLanguage(item, nestingLevel + 1);
            sb.Append(value);
        }

        sb.Append(" ]");
        return sb.ToString();
    }

    private bool IsValidName(string name)
    {
        return Regex.IsMatch(name, "^[a-zA-Z][a-zA-Z0-9]*$");
    }

    private string EscapeString(string str)
    {
        return str.Replace("'", "\\'");
    }
}