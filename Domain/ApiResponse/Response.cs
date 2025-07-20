using System.Net;

namespace Domain.ApiResponse;

public class Response<T>
{
    public bool IsSuccess { get; set; }
    public string? Messenge { get; set; }
    public T? Data { get; set; }
    public int StatusCode { get; set; }

    public Response(T? data, string? messenge = null)
    {
        IsSuccess = true;
        Messenge = messenge;
        Data = data;
        StatusCode = (int)HttpStatusCode.OK;
    }
    public Response(string messenge, HttpStatusCode statusCode)
    {
        IsSuccess = false;
        Messenge = messenge;
        Data = default;
        StatusCode = (int)statusCode;
    }

    public static Response<T> Success(T? data = default, string? messenge = null)
    {
        return new Response<T>(data, messenge);
    }

    public static Response<T> Error(string messenge, HttpStatusCode statusCode)
    {
        return new Response<T>(messenge, statusCode);
    }
}
