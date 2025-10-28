using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace SampleApi.Presentation.Configurations.Swagger
{
  public class SwaggerGenOptionsSetup : IConfigureOptions<SwaggerGenOptions>
  {
    private readonly IApiVersionDescriptionProvider _provider;

    public SwaggerGenOptionsSetup(IApiVersionDescriptionProvider provider)
    {
      _provider = provider;
    }

    public void Configure(SwaggerGenOptions options)
    {
      foreach (var description in _provider.ApiVersionDescriptions)
      {
        options.SwaggerDoc(description.GroupName, new OpenApiInfo
        {
          Title = "Sample API",
          Version = description.ApiVersion.ToString(),
          Description = "A versioned ASP.NET Core API with JWT authentication and Swagger documentation",
          Contact = new OpenApiContact
          {
            Name = "RizeForge DevOps Team",
            Url = new Uri("https://rizeforge.dev")
          }
        });
      }

      options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
      {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter a valid JWT token as: Bearer {your_token}"
      });

      options.AddSecurityRequirement(new OpenApiSecurityRequirement
      {
        {
          new OpenApiSecurityScheme
            {
              Reference = new OpenApiReference
              {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
              }
            },
          Array.Empty<string>()
        }
      });

      options.SchemaFilter<ApiResponseSchemaFilter>();
    }
  }
}
