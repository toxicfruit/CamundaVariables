using CamundaVariables.Library.Extensions;
using CamundaVariables.Library.VariableModels;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CamundaVariables.Library.Serialization;

public class DateTimeVariableJsonConverter: JsonConverter<DateTimeVariable>
{
    public override DateTimeVariable? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return null;
        }

        using var jsonDocument = JsonDocument.ParseValue(ref reader);
        var rootElement = jsonDocument.RootElement;
        var variableType = rootElement.GetProperty("type").GetString();

        return variableType switch
        {
            "Data" => rootElement.Deserialize<DateTimeVariable>(options),
            _ => throw new JsonException($"Unable to deserialize {variableType} to DateTimeVariable")
        };

    }

    public override void Write(Utf8JsonWriter writer, DateTimeVariable value, JsonSerializerOptions options)
    {
        var body = new { value = value.Value.ToJavaISO8601() };
        var jsonNode = JsonSerializer.SerializeToNode(body, body.GetType(), options) ?? throw new JsonException();

        jsonNode.WriteTo(writer, options);
    }
}
