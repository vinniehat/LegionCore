using System.Net;

namespace LegionCore.Core.Models.Api;

public class ApiResponse
{
    public HttpStatusCode HttpStatusCode { get; set; } = HttpStatusCode.OK;
    public object? Data { get; set; }
    public string? Message { get; set; }

    
    public ApiResponse(HttpStatusCode httpStatusCode, object? data = null, string? message = "")
    {
        HttpStatusCode = httpStatusCode;
        Data = data;
        Message = message;
    }
}