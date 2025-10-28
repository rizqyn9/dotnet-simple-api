using SampleApi.Presentation.Middlewares;

namespace SampleApi.Presentation.Extensions
{
  public static class ApplicationBuilderExtensions
  {
    /// <summary>
    /// Registers global middleware like exception handling, logging, etc.
    /// </summary>
    public static IApplicationBuilder UsePresentation(this IApplicationBuilder app)
    {
      app.UseMiddleware<ExceptionHandlingMiddleware>();

      // Security & routing middlewares
      app.UseHttpsRedirection();
      app.UseCors("DefaultPolicy");
      app.UseAuthentication();
      app.UseAuthorization();

      return app;
    }
  }
}
