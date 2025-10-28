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

        if (valueType == null)
          return;

        // Skip wrapping if already ApiResponse<T>
        if (valueType.IsGenericType &&
            valueType.GetGenericTypeDefinition() == typeof(ApiResponse<>))
        {
          return;
        }

        // Skip wrapping if it's an ErrorResponse
        if (valueType == typeof(ErrorResponse))
        {
          return;
        }

        // Otherwise wrap as ApiResponse<object>
        var wrapped = ApiResponse<object>.SuccessResponse(objectResult.Value!);
        context.Result = new ObjectResult(wrapped)
        {
          StatusCode = objectResult.StatusCode
        };
      }
    }

  }
}
