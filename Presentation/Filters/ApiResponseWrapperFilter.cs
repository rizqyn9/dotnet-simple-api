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
      if (context.Result is not ObjectResult objectResult)
        return;

      var value = objectResult.Value;
      if (value == null)
        return;

      var traceId = context.HttpContext.TraceIdentifier;
      var valueType = value.GetType();

      if (IsAlreadyWrapped(valueType))
      {
        SetTraceId(value, valueType, traceId);
        return;
      }

      var wrapped = ResponseFactory.Success(value, traceId: traceId);
      context.Result = new ObjectResult(wrapped)
      {
        StatusCode = objectResult.StatusCode
      };
    }

    private static bool IsAlreadyWrapped(Type valueType)
    {
      if (valueType.IsGenericType && valueType.GetGenericTypeDefinition() == typeof(ApiResponse<>))
        return true;

      return valueType == typeof(ErrorResponse);
    }

    private static void SetTraceId(object target, Type targetType, string traceId)
    {
      var traceIdProperty = targetType.GetProperty(nameof(BaseResponse.TraceId));
      traceIdProperty?.SetValue(target, traceId);
    }
  }
}
