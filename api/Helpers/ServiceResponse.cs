namespace api.Helpers;

using System.Net;

public class ServiceResponse<T>
{
    public T? Data { get; set; }
    public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;
    public string? Message { get; set; } = "Success";
}