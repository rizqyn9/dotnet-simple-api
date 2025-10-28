using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

using SampleApi.Presentation.Common.Responses;

namespace SampleApi.Presentation.Filters
{
  public class ApiResponseWrapperFilter : IActionFilter
  {
    public void OnActionExecuting(ActionExecutingContext context) { }

    public void OnActionExecuted(ActionExecutedContext context)
    {
      if (context.Result is ObjectResult objectResult)
      {
        var valueType = objectResult.Value?.GetType();
        var httpContext = context.HttpContext;
        var traceId = httpContext.TraceIdentifier;

        if (valueType == null)
          return;

        if (valueType.IsGenericType &&
            valueType.GetGenericTypeDefinition() == typeof(ApiResponse<>))
        {
          var traceIdProp = valueType.GetProperty("TraceId");
          traceIdProp?.SetValue(objectResult.Value, traceId);
          return;
        }

        if (valueType == typeof(ErrorResponse))
        {
          var traceIdProp = valueType.GetProperty("TraceId");
          traceIdProp?.SetValue(objectResult.Value, traceId);
          return;
        }

        var wrapped = ApiResponse<object>.SuccessResponse(objectResult.Value!, traceId: traceId);
        context.Result = new ObjectResult(wrapped)
        {
          StatusCode = objectResult.StatusCode
        };
      }
    }
  }
}
