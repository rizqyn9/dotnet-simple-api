using System.Net;

using SampleApi.Presentation.Common.Exceptions;
using SampleApi.Presentation.Common.Responses;

namespace SampleApi.Presentation.Middlewares
{
  public class ExceptionHandlingMiddleware
  {
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
      _next = next;
      _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
      var traceId = context.TraceIdentifier;
      try
      {
        await _next(context);
      }
      catch (ApiException ex)
      {
        _logger.LogWarning(ex, "Handled API exception ({TraceId})", traceId);
        context.Response.StatusCode = ex.StatusCode;
        context.Response.ContentType = "application/json";

        var response = new ErrorResponse
        {
          Message = ex.Message,
          Code = ex.Code,
          Fields = ex is ValidationException vex ? vex.Errors : null,
          TraceId = traceId
        };

        await context.Response.WriteAsJsonAsync(response);
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Unhandled exception ({TraceId})", traceId);
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        context.Response.ContentType = "application/json";

        var response = new ErrorResponse
        {
          Message = "An unexpected error occurred.",
          Code = "INTERNAL_SERVER_ERROR",
          TraceId = traceId
        };

        await context.Response.WriteAsJsonAsync(response);
      }
    }
  }
}
