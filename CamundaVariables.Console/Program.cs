using CamundaVariables.Console;
using CamundaVariables.Library.Client;
using CamundaVariables.Library.Models;
using CamundaVariables.Library.VariableModels;
using RandomTestValues;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;

var token = Convert.ToBase64String(Encoding.UTF8.GetBytes($"demo:demo"));

var httpClient = new HttpClient
{
    BaseAddress = new Uri("http://localhost:8765/engine-rest"),
};
httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

var client = new CamundaClient(httpClient);

var objectValue = RandomValue.Object<ComplexType>();
var docObjectValue = ObjectToXDocument(objectValue);

var request = new DeliverMessageRequest
{
    MessageName = "StartWaitAndDie",
    ProcessVariables = new Variables()
        .WithVariable("BoolVariable", new BooleanVariable(RandomValue.Bool()))
        .WithVariable("BytesVariable", new BytesVariable(RandomValue.Array<Byte>()))
        .WithVariable("DoubleVariable", new DoubleVariable(RandomValue.Double()))
        .WithVariable("IntegerVariable", new IntegerVariable(RandomValue.Int()))
        .WithVariable("JsonVariable", new JsonVariable(JsonSerializer.SerializeToNode(objectValue)))
        .WithVariable("LongVariable", new LongVariable(RandomValue.Long()))
        .WithVariable("NullVariable", new NullVariable())
        .WithVariable("ObjectVariable", new ObjectVariable(objectValue))
        .WithVariable("ShortVariable", new ShortVariable(RandomValue.Short()))
        .WithVariable("StringVariable", new StringVariable(RandomValue.String()))
        .WithVariable("XmlVariable", new XmlVariable(docObjectValue))
        .WithVariable("ListVariable", new ObjectVariable(RandomValue.List<string>()))
};

try
{
    await client.DeliverMessageAsync(request);
}
catch (ClientException ex)
{
    Console.WriteLine(ex.Message);
}
catch (Exception ex)
{
    Console.WriteLine(ex.ToString());
}

static XDocument ObjectToXDocument(ComplexType objectValue)
{
    var doc = new XDocument();
    using (var writer = doc.CreateWriter())
    {
        var serializer = new DataContractSerializer(objectValue.GetType());
        serializer.WriteObject(writer, objectValue);
    }
    return doc;
}
