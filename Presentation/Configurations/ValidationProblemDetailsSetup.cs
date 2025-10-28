using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

using SampleApi.Presentation.Common.Responses;

namespace SampleApi.Presentation.Configurations
{
  public static class ValidationProblemDetailsSetup
  {
    public static IMvcBuilder ConfigureValidationResponse(this IMvcBuilder builder)
    {
      builder.Services.Configure<ApiBehaviorOptions>(options =>
      {
        options.InvalidModelStateResponseFactory = context =>
        {
          var errors = context.ModelState
            .Where(e => e.Value?.Errors.Count > 0)
            .ToDictionary(
              kvp => kvp.Key,
              kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
            );

          var response = new ErrorResponse
          {
            Success = false,
            Message = "Validation failed.",
            Code = "VALIDATION_FAILED",
            Fields = errors
          };

          return new BadRequestObjectResult(response);
        };
      });

      return builder;
    }
  }
}
