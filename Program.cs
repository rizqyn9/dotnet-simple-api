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
using SampleApi.Configurations;
using SampleApi.Infrastructure.Data;
using SampleApi.Infrastructure.Repositories;
using SampleApi.Middleware;


using Serilog;

Log.Logger = new LoggerConfiguration()
  .WriteTo.Console()
  .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
  .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRouting(options => options.LowercaseUrls = true);

// Add JWT Auth
var jwtKey = builder.Configuration["Jwt:Key"]!;
var keyBytes = Encoding.UTF8.GetBytes(jwtKey);

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
      options.TokenValidationParameters = new TokenValidationParameters
      {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(keyBytes)
      };
    });

builder.Services.AddApiVersioning(options =>
{
  options.DefaultApiVersion = new ApiVersion(1, 0);
  options.AssumeDefaultVersionWhenUnspecified = true;
  options.ReportApiVersions = true;
});

builder.Services.AddVersionedApiExplorer(options =>
{
  options.GroupNameFormat = "'v'VVV";
  options.SubstituteApiVersionInUrl = true;
});

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureOptions<SwaggerGenOptionsSetup>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Controllers + Validation
builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<CreateUserDtoValidator>();

// Dependency Injection
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthService, AuthService>();


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
  var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
  db.Database.Migrate();
}

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

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
