using System.Net;
using System.Text.Json;

namespace LibraryAPI.API.Middleware;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error no controlado: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        context.Response.ContentType = "application/json";

        var (statusCode, message) = ex switch
        {
            InvalidOperationException => (HttpStatusCode.BadRequest, ex.Message),
            KeyNotFoundException => (HttpStatusCode.NotFound, ex.Message),
            _ => (HttpStatusCode.InternalServerError, "Ocurrió un error interno en el servidor.")
        };

        context.Response.StatusCode = (int)statusCode;

        var response = new
        {
            status = (int)statusCode,
            error = message
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}