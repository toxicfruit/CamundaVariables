using CamundaVariables.Library.Models;
using CamundaVariables.Library.VariableModels;
using CamundaVariables.LibraryTests;
using CamundaVariables.LibraryTests.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RandomTestValues;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace CamundaVariables.Library.Client.Tests;

[TestClass()]
public class ClientTests
{
    [TestMethod()]
    [TestCategory("UnitTest")]
    public async Task DeliverMessageAsync_HttpException_ThrowsException()
    {
        var expectedError = RandomValue.Object<ErrorResponse>();

        var request = new DeliverMessageRequest
        {
            MessageName = "StartWaitAndDie"
        };

        var httpClient = FakeHttpClient.Create((request, ct) => 
            Task.FromResult(new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = JsonContent.Create(expectedError)
            }));

        var sut = new Client(httpClient);

        var actual = await Assert.ThrowsExceptionAsync<ClientException>(() => sut.DeliverMessageAsync(request));
        Assert.AreEqual(HttpStatusCode.BadRequest, actual.StatusCode);
        Assert.AreEqual(expectedError.Type, actual.ErrorType);
        Assert.AreEqual(expectedError.Message, actual.ErrorMessage);
    }

    [TestMethod()]
    [TestCategory("UnitTest")]
    public async Task DeliverMessageAsync_HttpClientReturnsSuccess_ReturnsSuccess()
    {
        var objectValue = RandomValue.Object<TestData>();
        var request = new DeliverMessageRequest
        {
            MessageName = "StartWaitAndDie",
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

        var httpClient = FakeHttpClient.Create((request, ct) => 
            Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = JsonContent.Create(request)
            })
        );

        var sut = new Client(httpClient);

        try
        {
            await sut.DeliverMessageAsync(request);
        }
        catch (Exception ex)
        {
            Assert.Fail(ex.Message);
        }
    }

    [TestMethod()]
    [TestCategory("Integration")]
    [Ignore]
    public async Task DeliverMessageAsyncTest()
    {
        var httpClient = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:8765/engine-rest")
        }
        .Also(httpClient =>
        {
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", Convert.ToBase64String(Encoding.UTF8.GetBytes($"demo:demo")));
        });

        var sut = new Client(httpClient);

        var objectValue = RandomValue.Object<TestData>();

        var request = new DeliverMessageRequest
        {
            MessageName = "StartWaitAndDie",
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
            await sut.DeliverMessageAsync(request);
        }
        catch (ClientException ex)
        {
            Assert.Fail(ex.ToString());
        }
    }

}