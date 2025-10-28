using Serilog;

namespace SampleApi.Presentation.Configurations.Logging
{
  public static class SerilogSetup
  {
    public static void ConfigureSerilog(WebApplicationBuilder builder)
    {
      var loggerConfig = new LoggerConfiguration()
          .Enrich.FromLogContext()
          .WriteTo.Console();

      if (builder.Environment.IsProduction())
      {
        loggerConfig.WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day);
      }

      Log.Logger = loggerConfig.CreateLogger();

      builder.Host.UseSerilog((context, services, configuration) =>
            {
              configuration
                .Enrich.FromLogContext()
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services)
                .Enrich.WithProperty("Application", "SampleApi")
                .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName);
            });
    }
  }
}
