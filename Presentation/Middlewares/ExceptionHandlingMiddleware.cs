using System.Net;
using System.Text.Json;

using SampleApi.Presentation.Common.Exceptions;
using SampleApi.Presentation.Common.Responses;

using Serilog;

namespace SampleApi.Presentation.Middlewares
{
  public class ExceptionHandlingMiddleware
  {
    private readonly RequestDelegate _next;
    private readonly Serilog.ILogger _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
      _next = next;
      _logger = Log.ForContext<ExceptionHandlingMiddleware>();
    }

    public async Task InvokeAsync(HttpContext context)
    {
      try
      {
        await _next(context);
      }
      catch (Exception ex)
      {
        await HandleExceptionAsync(context, ex);
      }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
      var traceId = context.TraceIdentifier;

      _logger.Error(ex, "Unhandled exception occurred (TraceId={TraceId})", traceId);

      var response = ResponseFactory.FromException(ex, traceId);
      var json = JsonSerializer.Serialize(response);

      context.Response.ContentType = "application/json";
      context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

      await context.Response.WriteAsync(json);
    }
  }
}
