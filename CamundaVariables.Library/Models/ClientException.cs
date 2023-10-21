using System;
using System.Net;

namespace CamundaVariables.Library.Models;

public class ClientException: Exception
{
    public readonly ErrorResponse _errorResponse;

    public HttpStatusCode StatusCode { get; }
    public string ErrorType => _errorResponse.Type;
    public string ErrorMessage => _errorResponse.Message;
    public override string Message => $"Camunda returned error of type \"{ErrorType}\" with message \"{ErrorMessage}\"";

    public ClientException(ErrorResponse errorResponse, HttpStatusCode statusCode)
    {
        _errorResponse = errorResponse;
        StatusCode = statusCode;
    }
}
