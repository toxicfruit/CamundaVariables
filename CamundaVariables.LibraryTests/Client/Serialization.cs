using CamundaVariables.Library.Models;
using CamundaVariables.Library.Serialization;
using CamundaVariables.Library.VariableModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RandomTestValues;
using System.Text.Json;

namespace CamundaVariables.LibraryTests.Client;

[TestClass]
[TestCategory("Unit")]
public class Serialization
{
    [TestMethod]
    public void DeserializeDeliverMessageRequestTest()
    {
        var jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        }
        .Also(options =>
        {
            options.Converters.Add(new JsonVariableJsonConverter());
            options.Converters.Add(new VariableJsonConverter());
            options.Converters.Add(new XmlVariableJsonConverter());
        });

        var objectValue = RandomValue.Object<TestData>();

        var request = new DeliverMessageRequest
        {
            MessageName = RandomValue.String(),
            ProcessVariables = new VariableBuilder()
                .WithVariable("BoolVariable", RandomValue.Bool())
                .WithVariable("BytesVariable", RandomValue.Array<byte>())
                .WithVariable("DoubleVariable", RandomValue.Double())
                .WithVariable("IntegerVariable", RandomValue.Int())
                .WithVariable("JsonVariable", JsonSerializer.SerializeToNode(objectValue))
                .WithVariable("LongVariable", RandomValue.Long())
                .WithVariable("NullVariable", new NullVariable())
                .WithVariable("ObjectVariable", objectValue)
                .WithVariable("ShortVariable", RandomValue.Short())
                .WithVariable("StringVariable", RandomValue.String())
                .WithVariable("XmlVariable", objectValue.ToXDocument())
                .WithVariable("ListVariable", RandomValue.List<string>())
                .Build()
        };

        try
        {
            var json = JsonSerializer.Serialize(request, jsonSerializerOptions);
            var deserialized = JsonSerializer.Deserialize<DeliverMessageRequest>(json, jsonSerializerOptions);
        }
        catch (Exception ex)
        {
            Assert.Fail(ex.Message);
        }
    }
}
