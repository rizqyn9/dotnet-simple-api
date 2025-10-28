using System.Text;

using FluentValidation;
using FluentValidation.AspNetCore;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using SampleApi.Application.Services;
using SampleApi.Application.Validators;
using SampleApi.Infrastructure.Data;
using SampleApi.Infrastructure.Repositories;
using SampleApi.Presentation.Configurations;
using SampleApi.Presentation.Configurations.Swagger;
using SampleApi.Presentation.Filters;

namespace SampleApi.Presentation.Extensions
{
  public static class ServiceCollectionExtensions
  {
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
      var connectionString = configuration.GetConnectionString("DefaultConnection")
          ?? throw new InvalidOperationException("Database connection string not found.");

      services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));

      services.AddScoped<IUserRepository, UserRepository>();

      return services;
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
      services.AddScoped<IUserService, UserService>();
      services.AddScoped<IAuthService, AuthService>();
      return services;
    }

    public static IServiceCollection AddPresentationLayer(this IServiceCollection services, IConfiguration configuration)
    {

      services.AddJwtAuthentication(configuration)
            .AddControllersWithFilters()
            .AddValidation()
            .AddApiVersioningAndSwagger();

      return services;
    }

    private static IServiceCollection AddJwtAuthentication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
      var jwtKey = configuration["Jwt:Key"]
          ?? throw new InvalidOperationException("JWT Key not configured.");
      var issuer = configuration["Jwt:Issuer"]
          ?? throw new InvalidOperationException("JWT Issuer not configured.");
      var audience = configuration["Jwt:Audience"]
          ?? throw new InvalidOperationException("JWT Audience not configured.");

      var keyBytes = Encoding.UTF8.GetBytes(jwtKey);

      services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
              .AddJwtBearer(options =>
              {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                  ValidateIssuer = true,
                  ValidateAudience = true,
                  ValidateIssuerSigningKey = true,
                  ValidIssuer = issuer,
                  ValidAudience = audience,
                  IssuerSigningKey = new SymmetricSecurityKey(keyBytes)
                };
              });

      return services;
    }

    private static IServiceCollection AddControllersWithFilters(this IServiceCollection services)
    {
      services.AddRouting(options => options.LowercaseUrls = true);
      services.AddControllers(options =>
      {
        // Global response wrapper
        options.Filters.Add<ApiResponseWrapperFilter>();
      });

      return services;
    }

    private static IServiceCollection AddValidation(this IServiceCollection services)
    {
      services.ConfigureValidationResponse();
      services.AddFluentValidationAutoValidation();
      services.AddFluentValidationClientsideAdapters();
      services.AddValidatorsFromAssemblyContaining<CreateUserDtoValidator>();
      return services;
    }

    private static IServiceCollection AddApiVersioningAndSwagger(this IServiceCollection services)
    {
      services.AddEndpointsApiExplorer();
      services.AddSwaggerGen();
      services.ConfigureOptions<SwaggerGenOptionsSetup>();

      services.AddApiVersioning(options =>
      {
        options.DefaultApiVersion = new ApiVersion(1, 0);
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.ReportApiVersions = true;
      });

      services.AddVersionedApiExplorer(options =>
      {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
      });

      return services;
    }
  }
}
