using CamundaVariables.Library.Variables;
using System;
using System.Collections.Generic;
using System.Net.Mime;
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
            "String" => rootElement.Deserialize<StringVariable>(options),
            "Boolean" => rootElement.Deserialize<BooleanVariable>(options),
            "Short" => rootElement.Deserialize<ShortVariable>(options),
            "Integer" => rootElement.Deserialize<IntegerVariable>(options),
            "Long" => rootElement.Deserialize<LongVariable>(options),
            "Double" => rootElement.Deserialize<DoubleVariable>(options),
            "Bytes" => rootElement.Deserialize<BytesVariable>(options),
            "Null" => rootElement.Deserialize<NullVariable>(options),
            "Json" => rootElement.Deserialize<JsonVariable>(options),
            "Xml" => rootElement.Deserialize<XmlVariable>(options),
            "Object" => rootElement.Deserialize<ObjectVariable>(options),
            _ => rootElement.Deserialize<UnknownVariable>(options)
        };
    }

    public override void Write(Utf8JsonWriter writer, VariableBase value, JsonSerializerOptions options)
    {
        var jsonNode = JsonSerializer.SerializeToNode(value, value.GetType(), options)
            ?? throw new JsonException();

        jsonNode["type"] = value switch
        {
            StringVariable => "String",
            BooleanVariable => "Boolean",
            ShortVariable => "Short",
            IntegerVariable => "Integer",
            LongVariable => "Long",
            DoubleVariable => "Double",
            BytesVariable => "Bytes",
            NullVariable => "Null",
            JsonVariable => "Json",
            XmlVariable => "Xml",
            UnknownVariable unknownVariable => unknownVariable.Type,
            ObjectVariable => null,
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
        };

        //if (value is ObjectVariable)
        //{
        //    jsonNode["valueInfo"] = JsonSerializer.SerializeToNode(_valueInfo, _valueInfo.GetType(), options);
        //}

        jsonNode.WriteTo(writer, options);
    }
}
