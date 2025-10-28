using Microsoft.AspNetCore.Mvc;

using SampleApi.Presentation.Common.Responses;

namespace SampleApi.Presentation.Configurations
{
  public static class ValidationProblemDetailsSetup
  {
    public static IServiceCollection ConfigureValidationResponse(this IServiceCollection services)
    {
      services.Configure<ApiBehaviorOptions>(options =>
      {
        options.InvalidModelStateResponseFactory = context =>
        {
          var errors = context.ModelState
            .Where(e => e.Value?.Errors.Count > 0)
            .ToDictionary(
              kvp => kvp.Key,
              kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
            );

          var traceId = context.HttpContext.TraceIdentifier;

          var response = new ErrorResponse
          {
            Message = "Validation failed.",
            Code = "VALIDATION_FAILED",
            Fields = errors,
            TraceId = traceId
          };

          return new BadRequestObjectResult(response);
        };
      });

      return services;
    }
  }
}
