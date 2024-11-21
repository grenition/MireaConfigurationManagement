using System.Text;
using System.Text.RegularExpressions;

namespace MireaConfigurationManagement.ConfLanguage;

public class JsonToConfConverter
{
    private StringBuilder _output;
    
    public string ConvertFromJson(string json)
    {
        _output = new StringBuilder();

        var parser = new JsonCommentParser();
        var rootNodes = parser.Parse(json);
        
        foreach (var rootNode in rootNodes)
        {
            ConvertJsonNodeToConfLanguage(rootNode);
        }
        
        return _output.ToString();
    }
    private void ConvertJsonNodeToConfLanguage(JsonNode node, int indentLevel = 0)
    {
        switch (node.Type)
        {
            case JsonNodeType.Comment:
                WriteComment(node.Value, indentLevel);
                break;
            case JsonNodeType.Object:
                ConvertObjectNode(node, indentLevel);
                break;
            case JsonNodeType.Array:
                ConvertArrayNode(node, indentLevel);
                break;
            case JsonNodeType.Property:
                ConvertPropertyNode(node, indentLevel);
                break;
            case JsonNodeType.Value:
                ConvertValueNode(node, indentLevel);
                break;
            default:
                throw new Exception($"Unsupported node type: {node.Type}");
        }
    }

    private void ConvertObjectNode(JsonNode node, int indentLevel)
    {
        AppendIndent(_output, indentLevel);
        _output.AppendLine("table(");

        bool first = true;
        foreach (var child in node.Children)
        {
            if (!first)
                _output.AppendLine(",");
            first = false;

            ConvertJsonNodeToConfLanguage(child, indentLevel + 1);
        }

        _output.AppendLine();
        AppendIndent(_output, indentLevel);
        _output.Append(")");
    }

    private void ConvertArrayNode(JsonNode node, int indentLevel)
    {
        AppendIndent(_output, indentLevel);
        _output.AppendLine("[");

        bool first = true;
        foreach (var child in node.Children)
        {
            if (!first)
                _output.AppendLine(";");
            first = false;

            ConvertJsonNodeToConfLanguage(child, indentLevel + 1);
        }

        _output.AppendLine();
        AppendIndent(_output, indentLevel);
        _output.Append("]");
    }

    private void ConvertPropertyNode(JsonNode node, int indentLevel)
    {
        AppendIndent(_output, indentLevel);

        var name = node.Name;
        if (!IsValidName(name))
            throw new Exception($"Invalid name: {name}");

        var valueOutput = new StringBuilder();
        ConvertJsonNodeToConfLanguage(node.ValueNode, 0);

        _output.Append($"{name} => {valueOutput}");
    }

    private void ConvertValueNode(JsonNode node, int indentLevel)
    {
        switch (node.ValueKind)
        {
            case JsonValueKind.String:
                _output.Append($"'{EscapeString(node.Value)}'");
                break;
            case JsonValueKind.Number:
            case JsonValueKind.Boolean:
            case JsonValueKind.Null:
                _output.Append(node.Value);
                break;
            default:
                throw new Exception($"Unsupported value kind: {node.ValueKind}");
        }
    }

    private void WriteComment(string comment, int indentLevel)
    {
        AppendIndent(_output, indentLevel);
        _output.AppendLine("{#");
        foreach (var line in comment.Split(new[]
                 {
                     '\r', '\n'
                 }, StringSplitOptions.RemoveEmptyEntries))
        {
            AppendIndent(_output, indentLevel);
            _output.AppendLine(line.Trim());
        }
        AppendIndent(_output, indentLevel);
        _output.AppendLine("#}");
    }

    private bool IsValidName(string name)
    {
        return Regex.IsMatch(name, "^[a-zA-Z][a-zA-Z0-9]*$");
    }

    private string EscapeString(string str)
    {
        return str.Replace("'", "\\'");
    }

    private void AppendIndent(StringBuilder sb, int indentLevel)
    {
        sb.Append(new string(' ', indentLevel * 2));
    }
}
