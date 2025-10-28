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
      services.AddDbContext<ApplicationDbContext>(options =>
          options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

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
      // JWT setup
      var jwtKey = configuration["Jwt:Key"]!;
      var keyBytes = Encoding.UTF8.GetBytes(jwtKey);

      services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
              .AddJwtBearer(options =>
              {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                  ValidateIssuer = true,
                  ValidateAudience = true,
                  ValidateIssuerSigningKey = true,
                  ValidIssuer = configuration["Jwt:Issuer"],
                  ValidAudience = configuration["Jwt:Audience"],
                  IssuerSigningKey = new SymmetricSecurityKey(keyBytes)
                };
              });

      services.AddRouting(options => options.LowercaseUrls = true);
      services.AddControllers(options =>
      {
        options.Filters.Add<ApiResponseWrapperFilter>();
      });

      // Validation setup
      services.ConfigureValidationResponse();
      services.AddFluentValidationAutoValidation();
      services.AddFluentValidationClientsideAdapters();
      services.AddValidatorsFromAssemblyContaining<CreateUserDtoValidator>();

      // Swagger + API versioning
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
