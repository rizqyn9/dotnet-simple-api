using System.Text;

using FluentValidation;
using FluentValidation.AspNetCore;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using SampleApi.Application.Services;
using SampleApi.Application.Validators;
using SampleApi.Infrastructure.Data;
using SampleApi.Infrastructure.Repositories;
using SampleApi.Presentation.Configurations.Swagger;
using SampleApi.Presentation.Extensions;
using SampleApi.Presentation.Middlewares;

using Serilog;

Log.Logger = new LoggerConfiguration()
  .WriteTo.Console()
  .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
  .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Services
  .AddInfrastructureServices(builder.Configuration)
  .AddApplicationServices()
  .AddPresentationLayer(builder.Configuration);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
  var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
  db.Database.Migrate();
}

app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI(options =>
  {
    var apiVersionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

    foreach (var description in apiVersionProvider.ApiVersionDescriptions)
    {
      options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
            description.GroupName.ToUpperInvariant());
    }

    options.DocumentTitle = "Sample API Docs";
    options.RoutePrefix = string.Empty;
  });

  app.MapGet("/swagger/index.html", context =>
  {
    context.Response.Redirect("/");
    return Task.CompletedTask;
  });
}

app.MapControllers();
app.Run();
