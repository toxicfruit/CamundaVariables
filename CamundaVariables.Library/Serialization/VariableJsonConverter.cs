using CamundaVariables.Library.VariableModels;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CamundaVariables.Library.Serialization;

public class VariableJsonConverter: JsonConverter<VariableBase>
{
    public override VariableBase? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
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
            "Boolean" => rootElement.Deserialize<BooleanVariable>(options),
            "Bytes" => rootElement.Deserialize<BytesVariable>(options),
            "Date" => rootElement.Deserialize<DateTimeVariable>(options),
            "Double" => rootElement.Deserialize<DoubleVariable>(options),
            "Integer" => rootElement.Deserialize<IntegerVariable>(options),
            "Json" => rootElement.Deserialize<JsonVariable>(options),
            "Long" => rootElement.Deserialize<LongVariable>(options),
            "Null" => rootElement.Deserialize<NullVariable>(options),
            "Object" => rootElement.Deserialize<ObjectVariable>(options),
            "Short" => rootElement.Deserialize<ShortVariable>(options),
            "String" => rootElement.Deserialize<StringVariable>(options),
            "Xml" => rootElement.Deserialize<XmlVariable>(options),
            _ => rootElement.Deserialize<UnknownVariable>(options)
        };
    }

    public override void Write(Utf8JsonWriter writer, VariableBase value, JsonSerializerOptions options)
    {
        var jsonNode = JsonSerializer.SerializeToNode(value, value.GetType(), options)
            ?? throw new JsonException();

        jsonNode["type"] = value switch
        {
            BooleanVariable => "Boolean",
            BytesVariable => "Bytes",
            DateTimeVariable => "Date",
            DoubleVariable => "Double",
            IntegerVariable => "Integer",
            JsonVariable => "Json",
            LongVariable => "Long",
            NullVariable => "Null",
            ObjectVariable => null,
            ShortVariable => "Short",
            StringVariable => "String",
            UnknownVariable unknownVariable => unknownVariable.Type,
            XmlVariable => "Xml",
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
        };

        jsonNode.WriteTo(writer, options);
    }
}
