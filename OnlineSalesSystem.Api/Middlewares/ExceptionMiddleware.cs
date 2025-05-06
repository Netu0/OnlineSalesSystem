using System.Net;
using System.Text.Json;
using OnlineSalesSystem.Core.Exceptions;

namespace OnlineSalesSystem.Api.Middlewares;

public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<ExceptionMiddleware> _logger = logger;
/*
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception has occurred");
            await HandleExceptionAsync(context, ex);
        }
    }
*/
    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        // Tratamento específico para diferentes tipos de exceção
        var (statusCode, message) = exception switch
        {
            CustomerNotFoundException => (HttpStatusCode.NotFound, exception.Message),
            OrderNotFoundException => (HttpStatusCode.NotFound, exception.Message),
            InvalidOrderOperationException => (HttpStatusCode.BadRequest, exception.Message),
            _ => (HttpStatusCode.InternalServerError, "An unexpected error occurred")
        };

        var response = new 
        {
            StatusCode = (int)statusCode,
            Message = message,
            Details = statusCode == HttpStatusCode.InternalServerError ? null : exception.ToString()
        };

        context.Response.StatusCode = (int)statusCode;
        return context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}