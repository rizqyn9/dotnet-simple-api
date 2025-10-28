using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;

using SampleApi.Infrastructure.Data;
using SampleApi.Presentation.Configurations.Logging;
using SampleApi.Presentation.Extensions;
using SampleApi.Presentation.Middlewares;

using Serilog;

try
{
  Log.Information("Starting up Sample API...");

  var builder = WebApplication.CreateBuilder(args);

  SerilogSetup.ConfigureSerilog(builder);

  builder.Services
      .AddInfrastructureServices(builder.Configuration)
      .AddApplicationServices()
      .AddPresentationLayer(builder.Configuration);

  var app = builder.Build();

  // Run EF Core migrations
  using (var scope = app.Services.CreateScope())
  {
    try
    {
      var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
      db.Database.Migrate();
      Log.Information("✅ Database migration completed successfully.");
    }
    catch (Exception ex)
    {
      Log.Error(ex, "❌ An error occurred while applying database migrations.");
    }
  }


  // Middleware pipeline
  app.UseSerilogRequestLogging();
  app.UseMiddleware<ExceptionHandlingMiddleware>();

  app.UseHttpsRedirection();
  app.UseCors("DefaultPolicy");

  app.UseAuthentication();
  app.UseAuthorization();

  // Swagger for dev + staging
  if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
  {
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
      var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

      foreach (var description in provider.ApiVersionDescriptions)
      {
        options.SwaggerEndpoint(
                $"/swagger/{description.GroupName}/swagger.json",
                description.GroupName.ToUpperInvariant()
            );
      }

      options.DocumentTitle = "Sample API Documentation";
      options.RoutePrefix = string.Empty;
    });

    app.MapGet("/swagger/index.html", context =>
    {
      context.Response.Redirect("/", permanent: true);
      return Task.CompletedTask;
    });
  }

  app.MapControllers();

  app.Run();

  Log.Information("✅ Sample API started successfully.");
}
catch (Exception ex)
{
  Log.Fatal(ex, "❌ API terminated unexpectedly during startup.");
}
finally
{
  Log.Information("Shutting down Sample API...");
  Log.CloseAndFlush();
}
