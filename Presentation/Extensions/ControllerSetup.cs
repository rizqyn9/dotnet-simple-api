using SampleApi.Presentation.Configurations;
using SampleApi.Presentation.Filters;

namespace SampleApi.Presentation.Extensions
{
  public static class ControllerSetup
  {
    public static IServiceCollection AddPresentationControllers(this IServiceCollection services)
    {
      services.AddControllers(options =>
      {
        options.Filters.Add<ApiResponseWrapperFilter>();
      });

      services.ConfigureValidationResponse();
      return services;
    }
  }
}
