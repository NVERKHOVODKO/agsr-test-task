using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Patient.Core.Exceptions.Base;

namespace Patient.Core.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly IHostEnvironment _env;

    public ExceptionMiddleware(
        RequestDelegate next,
        ILogger<ExceptionMiddleware> logger,
        IHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        var response = context.Response;

        var errorDetails = new ErrorDetails();
        
        switch (exception)
        {
            case ApiException ex:
                response.StatusCode = ex.StatusCode;
                errorDetails.Message = ex.Message;
                errorDetails.Details = _env.IsDevelopment() ? ex.StackTrace : null;
                errorDetails.AdditionalData = ex.AdditionalData;
                break;
                
            case KeyNotFoundException ex:
                response.StatusCode = (int)HttpStatusCode.NotFound;
                errorDetails.Message = ex.Message;
                errorDetails.Details = _env.IsDevelopment() ? ex.StackTrace : null;
                break;
                
            case ArgumentException ex:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                errorDetails.Message = ex.Message;
                errorDetails.Details = _env.IsDevelopment() ? ex.StackTrace : null;
                break;
                
            default:
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                errorDetails.Message = "Internal Server Error";
                errorDetails.Details = _env.IsDevelopment() ? exception.StackTrace : null;
                break;
        }

        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        var json = JsonSerializer.Serialize(errorDetails, options);
        
        await context.Response.WriteAsync(json);
    }
}

public class ErrorDetails
{
    public string Message { get; set; } = null!;
    public string? Details { get; set; }
    public object? AdditionalData { get; set; }
}