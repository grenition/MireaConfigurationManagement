using System.Text;

namespace MireaConfigurationManagement.ConfLanguage;

public enum JsonNodeType
{
    Object,
    Array,
    Property,
    Value,
    Comment
}
public enum JsonValueKind
{
    String,
    Number,
    Boolean,
    Null
}
public class JsonNode
{
    public JsonNodeType Type { get; set; }
    public string Name { get; set; }
    public string Value { get; set; }
    public JsonValueKind ValueKind { get; set; }
    public List<JsonNode> Children { get; set; } = new List<JsonNode>();
    public JsonNode ValueNode { get; set; }
}
public class JsonCommentParser
{
    private string _json;
    private int _position;
    private int _length;

    public List<JsonNode> Parse(string json)
    {
        _json = json;
        _position = 0;
        _length = json.Length;

        var nodes = new List<JsonNode>();

        while (_position < _length)
        {
            SkipWhitespaceAndCollectComments(out var comments);

            if (_position >= _length)
                break;

            var node = ParseValueOrComment();

            if (comments != null && comments.Count > 0)
            {
                nodes.AddRange(comments);
            }

            nodes.Add(node);
        }

        return nodes;
    }

    private JsonNode ParseValueOrComment()
    {
        SkipWhitespace();

        if (_position >= _length)
            throw new Exception("Unexpected end of JSON input");

        char c = _json[_position];

        if (c == '/')
        {
            var commentNode = ParseComment();
            return commentNode;
        }
        else
        {
            return ParseValue();
        }
    }

    private JsonNode ParseValue()
    {
        SkipWhitespace();

        if (_position >= _length)
            throw new Exception("Unexpected end of JSON input");

        char c = _json[_position];

        if (c == '{')
            return ParseObject();
        if (c == '[')
            return ParseArray();
        if (c == '"')
            return ParseString();
        if (char.IsDigit(c) || c == '-')
            return ParseNumber();
        if (_json.Substring(_position).StartsWith("true"))
            return ParseLiteral("true", JsonValueKind.Boolean);
        if (_json.Substring(_position).StartsWith("false"))
            return ParseLiteral("false", JsonValueKind.Boolean);
        if (_json.Substring(_position).StartsWith("null"))
            return ParseLiteral("null", JsonValueKind.Null);

        throw new Exception($"Unexpected character '{c}' at position {_position}");
    }

    private JsonNode ParseObject()
    {
        var node = new JsonNode
        {
            Type = JsonNodeType.Object
        };

        _position++;

        while (_position < _length)
        {
            SkipWhitespaceAndCollectComments(out var comments);

            if (_position < _length && _json[_position] == '}')
            {
                _position++;
                break;
            }

            if (_position >= _length)
                throw new Exception("Unexpected end of JSON input in object");

            var property = ParseProperty();

            if (comments != null && comments.Count > 0)
            {
                node.Children.AddRange(comments);
            }

            node.Children.Add(property);

            SkipWhitespace();

            if (_position < _length && _json[_position] == ',')
            {
                _position++;
            }
            else if (_position < _length && _json[_position] == '}')
            {
                _position++;
                break;
            }
            else
            {
                throw new Exception("Expected ',' or '}' in object");
            }
        }

        return node;
    }

    private JsonNode ParseArray()
    {
        var node = new JsonNode
        {
            Type = JsonNodeType.Array
        };

        _position++;

        while (_position < _length)
        {
            SkipWhitespaceAndCollectComments(out var comments);

            if (_position < _length && _json[_position] == ']')
            {
                _position++;
                break;
            }

            if (_position >= _length)
                throw new Exception("Unexpected end of JSON input in array");

            var valueNode = ParseValueOrComment();

            if (comments != null && comments.Count > 0)
            {
                node.Children.AddRange(comments);
            }

            node.Children.Add(valueNode);

            SkipWhitespace();

            if (_position < _length && _json[_position] == ',')
            {
                _position++;
            }
            else if (_position < _length && _json[_position] == ']')
            {
                _position++;
                break;
            }
            else
            {
                throw new Exception("Expected ',' or ']' in array");
            }
        }

        return node;
    }

    private JsonNode ParseProperty()
    {
        SkipWhitespaceAndCollectComments(out var comments);

        var nameNode = ParseString();
        var name = nameNode.Value;

        SkipWhitespace();

        if (_position >= _length || _json[_position] != ':')
            throw new Exception("Expected ':' after property name");

        _position++; 

        SkipWhitespaceAndCollectComments(out var valueComments);

        var valueNode = ParseValueOrComment();

        var propertyNode = new JsonNode
        {
            Type = JsonNodeType.Property,
            Name = name,
            ValueNode = valueNode
        };
        
        if (comments != null && comments.Count > 0)
        {
            propertyNode.Children.InsertRange(0, comments);
        }

        if (valueComments != null && valueComments.Count > 0)
        {
            if (valueNode.Type == JsonNodeType.Comment)
            {
                propertyNode.Children.AddRange(valueComments);
            }
            else
            {
                if (valueNode.Children == null)
                    valueNode.Children = new List<JsonNode>();
                valueNode.Children.InsertRange(0, valueComments);
            }
        }

        return propertyNode;
    }

    private JsonNode ParseString()
    {
        var node = new JsonNode
        {
            Type = JsonNodeType.Value,
            ValueKind = JsonValueKind.String
        };

        var sb = new StringBuilder();
        _position++;

        while (_position < _length)
        {
            char c = _json[_position++];

            if (c == '\\')
            {
                if (_position >= _length)
                    throw new Exception("Unexpected end of string");

                char escape = _json[_position++];
                sb.Append(escape);
            }
            else if (c == '"')
            {
                // End of string
                node.Value = sb.ToString();
                return node;
            }
            else
            {
                sb.Append(c);
            }
        }

        throw new Exception("Unexpected end of string");
    }

    private JsonNode ParseNumber()
    {
        var node = new JsonNode
        {
            Type = JsonNodeType.Value,
            ValueKind = JsonValueKind.Number
        };

        var start = _position;

        while (_position < _length && (char.IsDigit(_json[_position]) || _json[_position] == '-' || _json[_position] == '+' || _json[_position] == '.' || _json[_position] == 'e' || _json[_position] == 'E'))
        {
            _position++;
        }

        node.Value = _json.Substring(start, _position - start);
        return node;
    }

    private JsonNode ParseLiteral(string literal, JsonValueKind kind)
    {
        var node = new JsonNode
        {
            Type = JsonNodeType.Value,
            ValueKind = kind
        };

        if (!_json.Substring(_position).StartsWith(literal))
            throw new Exception($"Expected '{literal}'");

        node.Value = literal;
        _position += literal.Length;
        return node;
    }

    private JsonNode ParseComment()
    {
        if (_position + 1 >= _length)
            throw new Exception("Unexpected end of JSON input in comment");

        char next = _json[_position + 1];

        if (next == '/')
        {
            _position += 2;
            var start = _position;
            while (_position < _length && _json[_position] != '\n')
            {
                _position++;
            }
            var comment = _json.Substring(start, _position - start);

            var commentNode = new JsonNode
            {
                Type = JsonNodeType.Comment,
                Value = comment.Trim()
            };

            return commentNode;
        }
        else if (next == '*')
        {
            _position += 2;
            var start = _position;
            while (_position + 1 < _length && !(_json[_position] == '*' && _json[_position + 1] == '/'))
            {
                _position++;
            }
            if (_position + 1 >= _length)
                throw new Exception("Unterminated comment");

            var comment = _json.Substring(start, _position - start);

            var commentNode = new JsonNode
            {
                Type = JsonNodeType.Comment,
                Value = comment.Trim()
            };

            _position += 2;

            return commentNode;
        }
        else
        {
            throw new Exception("Invalid comment");
        }
    }

    private void SkipWhitespaceAndCollectComments(out List<JsonNode> comments)
    {
        comments = null;

        while (_position < _length)
        {
            char c = _json[_position];

            if (char.IsWhiteSpace(c))
            {
                _position++;
            }
            else if (c == '/')
            {
                var commentNode = ParseComment();
                if (comments == null)
                    comments = new List<JsonNode>();
                comments.Add(commentNode);
            }
            else
            {
                break;
            }
        }
    }

    private void SkipWhitespace()
    {
        while (_position < _length && char.IsWhiteSpace(_json[_position]))
        {
            _position++;
        }
    }
}
