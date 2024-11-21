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
            ConvertJsonNodeToConfLanguage(rootNode, _output, 0);
        }

        return _output.ToString();
    }
    private void ConvertJsonNodeToConfLanguage(JsonNode node, StringBuilder output, int indentLevel)
    {
        switch (node.Type)
        {
            case JsonNodeType.Comment:
                WriteComment(node.Value, output, indentLevel);
                break;
            case JsonNodeType.Object:
                ConvertObjectNode(node, output, indentLevel);
                break;
            case JsonNodeType.Array:
                ConvertArrayNode(node, output, indentLevel);
                break;
            case JsonNodeType.Property:
                ConvertPropertyNode(node, output, indentLevel);
                break;
            case JsonNodeType.Value:
                ConvertValueNode(node, output, indentLevel);
                break;
            default:
                throw new Exception($"Unsupported node type: {node.Type}");
        }
    }

    private void ConvertObjectNode(JsonNode node, StringBuilder output, int indentLevel)
    {
        output.AppendLine("table(");

        bool first = true;
        JsonNode previous = null;
        foreach (var child in node.Children)
        {
            if (!first && (previous == null || previous.Type != JsonNodeType.Comment))
                output.AppendLine(",");
            first = false;

            ConvertJsonNodeToConfLanguage(child, output, indentLevel + 1);
            previous = child;
        }

        output.AppendLine();
        AppendIndent(output, indentLevel);
        output.Append(")");
    }

    private void ConvertArrayNode(JsonNode node, StringBuilder output, int indentLevel)
    {
        output.Append("[");

        bool first = true;
        foreach (var child in node.Children)
        {
            if (!first)
                output.Append("; ");
            first = false;

            ConvertJsonNodeToConfLanguage(child, output, indentLevel + 1);
        }

        output.Append("]");
    }

    private void ConvertPropertyNode(JsonNode node, StringBuilder output, int indentLevel)
    {
        AppendIndent(output, indentLevel);

        var name = node.Name;
        if (!IsValidName(name))
            throw new Exception($"Invalid name: {name}");

        var valueOutput = new StringBuilder();
        ConvertJsonNodeToConfLanguage(node.ValueNode, valueOutput, indentLevel);

        output.Append($"{name} => {valueOutput}");
    }

    private void ConvertValueNode(JsonNode node, StringBuilder output, int indentLevel)
    {
        switch (node.ValueKind)
        {
            case JsonValueKind.String:
                output.Append($"'{EscapeString(node.Value)}'");
                break;
            case JsonValueKind.Number:
            case JsonValueKind.Boolean:
            case JsonValueKind.Null:
                output.Append(node.Value);
                break;
            default:
                throw new Exception($"Unsupported value kind: {node.ValueKind}");
        }
    }

    private void WriteComment(string comment, StringBuilder output, int indentLevel)
    {
        AppendIndent(output, indentLevel);
        output.AppendLine("{#");
        foreach (var line in comment.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
        {
            AppendIndent(output, indentLevel);
            output.AppendLine(line.Trim());
        }
        AppendIndent(output, indentLevel);
        output.AppendLine("#}");
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
        sb.Append(new string(' ', indentLevel * 4));
    }
}
