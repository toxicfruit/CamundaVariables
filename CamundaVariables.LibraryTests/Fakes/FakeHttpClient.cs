namespace CamundaVariables.LibraryTests.Fakes;

internal class FakeHttpClient : HttpMessageHandler
{
    public Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> SendAsyncBehavior =
        async (message, token) =>
            await Task.FromException<HttpResponseMessage>(new NotImplementedException());

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        return SendAsyncBehavior(request, cancellationToken);
    }

    public static HttpClient Create(Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> behavior)
    {
        var handler = new FakeHttpClient
        {
            SendAsyncBehavior = behavior
        };
        return new HttpClient(handler)
        {
            BaseAddress = new Uri("https://test/.com")
        };
    }
}
