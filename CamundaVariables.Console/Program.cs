using CamundaVariables.Console;
using CamundaVariables.Library.Client;
using CamundaVariables.Library.Models;
using CamundaVariables.Library.Variables;
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
    ProcessVariables = new Dictionary<string, VariableBase>
    {
        ["BoolVariable"] = new BooleanVariable(RandomValue.Bool()),
        ["BytesVariable"] = new BytesVariable(RandomValue.Array<Byte>()),
        ["DoubleVariable"] = new DoubleVariable(RandomValue.Double()),
        ["IntegerVariable"] = new IntegerVariable(RandomValue.Int()),
        ["JsonVariable"] = new JsonVariable(JsonSerializer.SerializeToNode(objectValue)),
        ["LongVariable"] = new LongVariable(RandomValue.Long()),
        ["NullVariable"] = new NullVariable(),
        ["ObjectVariable"] = new ObjectVariable(objectValue),
        ["ShortVariable"] = new ShortVariable(RandomValue.Short()),
        ["StringVariable"] = new StringVariable(RandomValue.String()),
        ["XmlVariable"] = new XmlVariable(docObjectValue),
        ["ListVariable"] = new ObjectVariable(RandomValue.List<string>()),
    }
};

try
{
    await client.DeliverMessageAsync(request);
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
