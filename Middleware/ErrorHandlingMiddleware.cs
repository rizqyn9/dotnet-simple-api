using System.Net;
using System.Text.Json;

namespace SampleApi.Middleware
{
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
        _logger.LogError(ex, "Unhandled exception occurred.");
        await HandleExceptionAsync(context, ex);
      }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
      var code = HttpStatusCode.InternalServerError;

      var result = JsonSerializer.Serialize(new
      {
        success = false,
        error = ex.Message,
        stackTrace = ex.StackTrace
      });

      context.Response.ContentType = "application/json";
      context.Response.StatusCode = (int)code;

      return context.Response.WriteAsync(result);
    }
  }
}
