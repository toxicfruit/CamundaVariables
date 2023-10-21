using CamundaVariables.Library.Extensions;
using CamundaVariables.Library.Models;
using CamundaVariables.Library.Serialization;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace CamundaVariables.Library.Client;

public class CamundaClient
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public CamundaClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
        ValidateHttpClient(httpClient);

        _jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        }
        .Also(options =>
        {
            options.Converters.Add(new JsonStringEnumConverter());
            options.Converters.Add(new VariableJsonConverter());
            options.Converters.Add(new JsonVariableJsonConverter());
            options.Converters.Add(new XmlVariableJsonConverter());
        });
    }

    public async Task DeliverMessageAsync(DeliverMessageRequest request, CancellationToken cancellationToken = default)
    {
        using var response = await SendRequestAsync("/message", request, cancellationToken);
        await EnsureSuccessAsync(response, cancellationToken);
    }

    private static void ValidateHttpClient(HttpClient httpClient)
    {
        if (httpClient.BaseAddress == null)
        {
            throw new ArgumentException("BaseAddress must be configured", nameof(httpClient));
        }
    }

    private async Task<HttpResponseMessage> SendRequestAsync<T>(string path, T body, CancellationToken cancellationToken = default)
        where T : notnull
    {
        var basePath = _httpClient.BaseAddress?.AbsolutePath.TrimEnd('/') ?? string.Empty;
        var requestPath = $"{basePath}/{path.TrimStart('/')}";
        var response = await _httpClient.PostAsJsonAsync(requestPath, body, _jsonSerializerOptions, cancellationToken);
        return response;
    }

    private async Task EnsureSuccessAsync(HttpResponseMessage response, CancellationToken cancellationToken = default)
    {
        if (!response.IsSuccessStatusCode && response.IsJson())
        {
            var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>(_jsonSerializerOptions, cancellationToken);
            response.Content.Dispose();
            throw new ClientException(errorResponse ?? new ErrorResponse(), response.StatusCode);
        }
    }
}
