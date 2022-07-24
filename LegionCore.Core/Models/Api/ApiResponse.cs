using System.Net;

namespace LegionCore.Core.Models.Api;

public class ApiResponse
{
    public HttpStatusCode HttpStatusCode { get; set; } = HttpStatusCode.OK;
    public string? Message { get; set; }
}