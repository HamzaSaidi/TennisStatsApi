using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using TennisStatsApi.Exception;
 
namespace TennisStatsApi.Middlewares;

public class GlobalExceptionHandler
{
    private readonly RequestDelegate _next;

    public GlobalExceptionHandler(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (System.Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, System.Exception exception)
    {
        HttpStatusCode statusCode;
        string message;

        switch (exception)
        {
            case NotFoundException notFoundEx:
                statusCode = HttpStatusCode.NotFound;
                message = notFoundEx.Message;
                break;

            case ValidationException validationEx:
                statusCode = HttpStatusCode.BadRequest;
                message = validationEx.Message;
                break;

            case UnauthorizedAccessException unauthorizedEx:
                statusCode = HttpStatusCode.Unauthorized;
                message = "Unauthorized access.";
                break;

            default:
                statusCode = HttpStatusCode.InternalServerError;
                message = "An unexpected error occurred.";
                break;
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var result = JsonSerializer.Serialize(new { error = message });
        return context.Response.WriteAsync(result);
    }
}